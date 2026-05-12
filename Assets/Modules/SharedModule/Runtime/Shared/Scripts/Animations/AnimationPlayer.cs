using System.Collections.Generic;
using Animancer;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations
{
    public class AnimationPlayer : MonoBehaviour
    {
        public IDictionary<int, ITransition> TargetUsedTransitionByLayer => _targetUsedTransitionByLayer;
        public IDictionary<int, AnimationClip> TargetUsedAnimationByLayer => _targetUsedAnimationByLayer;

        public IAnimationsByIdProvider TargetAnimationsByIdProvider { get; private set; }
        public ITransitionsByIdProvider TargetTransitionsByIdProvider { get; private set; }

        [SerializeField] private AnimancerComponent _animancerComponent;
        [field: SerializeReference] private IAnimationsByIdProvider _animationsByIdProvider;
        [field: SerializeReference] private ITransitionsByIdProvider _transitionsByIdProvider;

        private AnimancerState _lastState;
        private bool _isInitialized;

        private readonly Dictionary<int, ITransition> _targetUsedTransitionByLayer = new Dictionary<int, ITransition>();

        private readonly Dictionary<int, AnimationClip> _targetUsedAnimationByLayer =
            new Dictionary<int, AnimationClip>();

        private void Start()
        {
            TryInitialize();
        }

        public void SetAnimationsProvider(IAnimationsByIdProvider provider)
        {
            TargetAnimationsByIdProvider = provider;
        }

        public void SetTransitionsProvider(ITransitionsByIdProvider provider)
        {
            TargetTransitionsByIdProvider = provider;
        }

        public void SetEnableAnimator(bool isEnabled)
        {
            _animancerComponent.Animator.enabled = isEnabled;
        }

        public void SetAnimationsProviderAsDefault()
        {
            TargetAnimationsByIdProvider = _animationsByIdProvider;
        }

        public bool IsPlayingAntNotEnding(int id, bool useDefaultAnimationsByIdProvider = false)
        {
            var targetAnimationsByIdProvider = useDefaultAnimationsByIdProvider
                ? _animationsByIdProvider
                : TargetAnimationsByIdProvider;

            if (targetAnimationsByIdProvider == null)
            {
                return false;
            }

            var animationClip = targetAnimationsByIdProvider.GetAnimation(id);

            return animationClip != null && _animancerComponent.States.TryGet(
                                             _animancerComponent.Graph.GetKey(animationClip), out var state)
                                         && state.IsPlayingAndNotEnding();
        }

        public bool IsPlaying(int id)
        {
            if (TargetAnimationsByIdProvider == null)
            {
                return false;
            }

            var animationClip = (object)TargetAnimationsByIdProvider.GetAnimation(id) ??
                                TargetTransitionsByIdProvider.GetTransition(id);

            return animationClip != null && _animancerComponent.IsPlaying(animationClip);
        }

        public AnimancerState Play(int id, float fadeDuration, int layer = 0)
        {
            TryInitialize();

            if (TargetAnimationsByIdProvider == null)
            {
                return null;
            }

            SetEnableAnimator(true);

            AnimancerState state;

            var animationClip = TargetAnimationsByIdProvider.GetAnimation(id);

            if (animationClip != null)
            {
                state = _animancerComponent.Layers[layer].Play(animationClip, fadeDuration);
                _targetUsedAnimationByLayer.SetOrAdd(layer, animationClip);
            }
            else
            {
                var transition = TargetTransitionsByIdProvider?.GetTransition(id);

                if (transition == null)
                {
                    return null;
                }

                state = _animancerComponent.Layers[layer].Play(transition, fadeDuration);
                _targetUsedTransitionByLayer.SetOrAdd(layer, transition);
            }

            var isPlayingIt = IsPlaying(id);

            if (isPlayingIt)
                state.Time = 0;
            
            return state;
        }

        public void Stop(int layer = 0)
        {
            _animancerComponent.Layers[layer].Stop();
        }

        private void TryInitialize()
        {
            if (_isInitialized)
                return;

            TargetAnimationsByIdProvider ??= _animationsByIdProvider;
            TargetTransitionsByIdProvider ??= _transitionsByIdProvider;
            _animancerComponent.ActionOnDisable = AnimancerComponent.DisableAction.Pause;
            _isInitialized = true;
        }
    }
}