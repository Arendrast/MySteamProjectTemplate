using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class CharacterControllerPushHandlerController : IPushable
    {
        public event Action<bool> Pushed;

        public bool IsEndedMinimumThrowTime => _startExplosionTime.GetPastTimeInSeconds() >= _minimumThrowTime;
        public Vector3 Direction { get; private set; }
        public float TotalDistance => _curvedSpeedByTimeMovementCalculator.TotalDistance;

        private bool _isInvokedPushedInThisFrame;
        private long _startExplosionTime;
        private Vector3 _currentExplosionForce;


        private Vector3 _requestedDirection, _requestedEndPoint;
        private float _requestedSpeed, _requestedTime;

        private readonly ManyInvokableOneFrameCharacterController _characterController;
        private readonly float _forceMultiplier, _timeMultiplier, _minimumThrowTime;
        private readonly AnimationCurve _speedCurve;
        private readonly CapsuleOverlapObserver _capsuleOverlapObserver;
        private readonly CharacterControllerPushHandlerModel _handlerModel;

        private readonly bool _shouldInvokeOnlyOneExplosionInOneFrame;

        private readonly CurvedSpeedByTimeMovementCalculator _curvedSpeedByTimeMovementCalculator;
        private readonly bool _shouldDisableCapsuleOverlapObserverWhenIsInactive;

        public CharacterControllerPushHandlerController(ManyInvokableOneFrameCharacterController characterController,
            float forceMultiplier,
            float timeMultiplier, float minimumThrowTime, AnimationCurve speedCurve,
            CapsuleOverlapObserver capsuleOverlapObserver,
            CharacterControllerPushHandlerModel handlerModel, bool shouldDisableCapsuleOverlapObserverWhenIsInactive,
            bool shouldInvokeOnlyOneExplosionInOneFrame = true)
        {
            _characterController = characterController;
            _forceMultiplier = forceMultiplier;
            _speedCurve = speedCurve;
            _capsuleOverlapObserver = capsuleOverlapObserver;
            _handlerModel = handlerModel;
            _shouldDisableCapsuleOverlapObserverWhenIsInactive = shouldDisableCapsuleOverlapObserverWhenIsInactive;
            _shouldInvokeOnlyOneExplosionInOneFrame = shouldInvokeOnlyOneExplosionInOneFrame;
            _timeMultiplier = timeMultiplier;
            _minimumThrowTime = minimumThrowTime;
            _curvedSpeedByTimeMovementCalculator = new CurvedSpeedByTimeMovementCalculator();

            if (shouldDisableCapsuleOverlapObserverWhenIsInactive)
                capsuleOverlapObserver.enabled = false;
        }

        public void TryPush(Vector3 explosionCenter, float forceAtCenter, float forceAtEdge, float radius,
            bool isBlockingExplosion, bool shouldMoveByYAxis)
        {
            var direction = (_characterController.Controller.transform.position - explosionCenter).normalized;
            var distance = Vector3.Distance(_characterController.Controller.transform.position, explosionCenter);
            var normalizedDistance = distance / radius;
            var moveDistance = Mathf.Lerp(forceAtEdge, forceAtCenter, normalizedDistance) * _forceMultiplier;

            TryPush(moveDistance, shouldMoveByYAxis ? direction : direction.WithY(0), isBlockingExplosion);
        }

        public void TryPush(float moveDistance, Vector3 direction, bool isBlockingExplosion)
        {
            if (!_handlerModel.CanPush || moveDistance <= float.Epsilon ||
                _shouldInvokeOnlyOneExplosionInOneFrame && _isInvokedPushedInThisFrame)
            {
                return;
            }

            OnStartAsync();

            return;

            async void OnStartAsync()
            {
                _requestedTime = _timeMultiplier * moveDistance;
                _requestedEndPoint = _characterController.Controller.transform.position +
                                     direction.normalized * moveDistance;
                _requestedDirection = direction;
                _isInvokedPushedInThisFrame = true;
                _handlerModel.IsBlocking = isBlockingExplosion;
                Pushed?.Invoke(isBlockingExplosion);

                await UniTask.DelayFrame(1);

                _isInvokedPushedInThisFrame = false;
            }
        }

        public void Stop()
        {
            if (_shouldDisableCapsuleOverlapObserverWhenIsInactive)
                _capsuleOverlapObserver.enabled = false;

            _capsuleOverlapObserver.EventsProvider.Entered -= TryCancelOnCapsuleOverlapEnter;
            _handlerModel.IsPushed = false;
        }

        public async void OnStartMovementAsync()
        {
            _capsuleOverlapObserver.EventsProvider.Entered -= TryCancelOnCapsuleOverlapEnter;

            Direction = _requestedDirection;

            _curvedSpeedByTimeMovementCalculator.Configure(
                _characterController.Controller.transform.position,
                _requestedEndPoint, _speedCurve, _requestedTime, out var configured);

            if (!configured)
            {
                Stop();
                return;
            }

            _handlerModel.IsPushed = true;
            _capsuleOverlapObserver.enabled = true;

            _startExplosionTime = DateTime.Now.Ticks;
            _capsuleOverlapObserver.EventsProvider.Entered += TryCancelOnCapsuleOverlapEnter;

            await UniTask.WaitForSeconds(_minimumThrowTime);

            if (!_handlerModel.IsPushed || _capsuleOverlapObserver.CurrentOverlaps.Count == 0 ||
                _capsuleOverlapObserver.CurrentOverlaps.All(overlap => overlap.isTrigger))
            {
                return;
            }

            Stop();
        }

        public void TryMove(bool shouldForceContinueMove, out bool didEndMove)
        {
            _curvedSpeedByTimeMovementCalculator.TryUpdateMovement(out didEndMove,
                out var distanceToMoveThisFrame, out var pathProgress);

            if (!shouldForceContinueMove && didEndMove)
            {
                Stop();
                return;
            }

            _characterController.Move(Direction * distanceToMoveThisFrame);
        }

        async void TryCancelOnCapsuleOverlapEnter(Collider collider)
        {
            if (collider.isTrigger)
            {
                return;
            }

            if (_handlerModel.IsPushed && _startExplosionTime.GetPastTimeInSeconds() >= _minimumThrowTime)
            {
                Stop();
                return;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0,
                _minimumThrowTime - _startExplosionTime.GetPastTimeInSeconds())));

            if (_handlerModel.IsPushed && _capsuleOverlapObserver.CurrentOverlaps.Contains(collider))
            {
                Stop();
            }
        }
    }
}