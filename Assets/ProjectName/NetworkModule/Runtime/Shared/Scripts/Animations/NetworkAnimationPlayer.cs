using Animancer;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Animations;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Animations
{
    public class NetworkAnimationPlayer : NetworkBehaviour
    {
        public SyncVar<int> ServerLastPlayedAnimationId { get; } = new();
        public SyncVar<int> ServerLastPlayedLayerId { get; } = new();
        public SyncVar<bool> ServerIsStopped { get; } = new();

        [SerializeField] private AnimationPlayer _animationPlayer;
        [SerializeField] private bool _canPlayTheSameAnimationBeforeEnd = true;
        [SerializeField] private bool _playLastPlayedAnimationFromServer;

        private int _lastPlayedAnimationId;


        public override void OnStartNetwork()
        {
            if (!IsServerStarted && _playLastPlayedAnimationFromServer)
                TryStartPlay();
        }

        public void TryStartPlay()
        {
            if (!Owner.IsLocalClient)
            {
                if (ServerIsStopped.Value)
                {
                    LocalStop(0, false); // Пока оставил 0, на будущее можно повумнее сделать
                    return;
                }

                PlayAnimation(ServerLastPlayedAnimationId.Value, 0, ServerLastPlayedLayerId.Value);
            }
        }

        public bool IsPlayingAndNotEnding(int id)
        {
            return _animationPlayer.IsPlayingAntNotEnding(id);
        }

        public AnimancerState Play(int id, float fadeAnimation = 0, int layer = 0,
            bool useDefaultAnimationsById = false, bool playOnlyForLocal = false, bool onlyIfOwner = false)
        {
            if (onlyIfOwner && !IsOwner)
                return null;
            
            if (!playOnlyForLocal)
            {
                if (IsServerStarted)
                {
                    PlayAnimationObserverRpc(id, fadeAnimation, layer, useDefaultAnimationsById, LocalConnection);
                }
                else if (IsOwner)
                {
                    PlayAnimationServerRpc(id, fadeAnimation, layer, useDefaultAnimationsById, LocalConnection);
                }
                else
                {
                    Debug.LogWarning("Not owner and not server send start animation");
                }
            }

            if (IsServerStarted || IsOwner || playOnlyForLocal)
            {
                return TryPlayAnimation(id, fadeAnimation, layer, useDefaultAnimationsById);
            }

            return null;
        }

        public void Stop(int layer = 0, bool forLocal = true, bool disableAnimator = false, bool onlyIfOwner = false)
        {
            if (onlyIfOwner && !IsOwner)
                return;
            
            if (IsServerStarted)
            {
                StopObserverRpc(layer, LocalConnection, disableAnimator);
            }
            else if (IsOwner)
            {
                StopServerRpc(layer, LocalConnection, disableAnimator);
            }
            
            if (forLocal)
                LocalStop(layer, disableAnimator);
        }

        [ObserversRpc]
        private void StopObserverRpc(int layer, NetworkConnection except, bool disableAnimator)
        {
            if (except != null && LocalConnection.CustomEquals(except))
                return;
            
            LocalStop(layer, disableAnimator);
        }

        [ServerRpc(RequireOwnership = false)]
        private void StopServerRpc(int layer, NetworkConnection except, bool disableAnimator)
        {
            StopObserverRpc(layer, except, disableAnimator);
        }

        private void LocalStop(int layer, bool disableAnimator)
        {
            if (disableAnimator)
                _animationPlayer.SetEnableAnimator(false);
            
            _animationPlayer.Stop(layer);
            
            if (IsServerStarted)
                ServerIsStopped.Value = true;
        }

        [ServerRpc]
        private void PlayAnimationServerRpc(int id, float fadeAnimation, int layer, bool useDefaultAnimationById, NetworkConnection except)
        {
            PlayAnimationObserverRpc(id, fadeAnimation, layer, useDefaultAnimationById, except);
        }

        [ObserversRpc]
        private void PlayAnimationObserverRpc(int id, float fadeAnimation, int layer, bool useDefaultAnimationById, 
            NetworkConnection except = null)
        {
            if (except != null && LocalConnection.CustomEquals(except))
                return;

            TryPlayAnimation(id, fadeAnimation, layer, useDefaultAnimationById);
        }

        private AnimancerState TryPlayAnimation(int id, float fadeAnimation, int layer,
            bool useDefaultAnimationsById = false)
        {
            if (!_canPlayTheSameAnimationBeforeEnd && IsPlayingAndNotEnding(id))
            {
                return null;
            }

            return PlayAnimation(id, fadeAnimation, layer, useDefaultAnimationsById);
        }

        private AnimancerState PlayAnimation(int id, float fadeAnimation, int layer,
            bool useDefaultAnimationsById = false)
        {
            var animationsProvider = _animationPlayer.TargetAnimationsByIdProvider;
            
            if (useDefaultAnimationsById)
            {
                _animationPlayer.SetAnimationsProviderAsDefault();
            }

            if (IsServerStarted)
            {
                ServerLastPlayedAnimationId.Value = id;
                ServerLastPlayedLayerId.Value = layer;
                ServerIsStopped.Value = false;
            }

            var animationState = _animationPlayer.Play(id, fadeAnimation, layer);

            if (useDefaultAnimationsById)
            {
                _animationPlayer.SetAnimationsProvider(animationsProvider);
            }

            return animationState;

        }
    }
}