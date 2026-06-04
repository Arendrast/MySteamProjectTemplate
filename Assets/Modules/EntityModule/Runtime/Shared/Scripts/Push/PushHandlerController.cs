using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.OverlapModule.Runtime.Scripts;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

#if TWO_D
using ActualCollider = UnityEngine.Collider2D;

#else
using ActualCollider = UnityEngine.Collider;
#endif

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class PushHandlerController : IPushable
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

        private readonly Action<Vector3> _moveAction;
        private readonly Transform _movableTransform;
        private readonly float _forceMultiplier, _timeMultiplier, _minimumThrowTime;
        private readonly AnimationCurve _speedCurve;
        private readonly OverlapObserver _overlapObserver;
        private readonly PushHandlerModel _handlerModel;

        private readonly bool _shouldInvokeOnlyOneExplosionInOneFrame;

        private readonly CurvedSpeedByTimeMovementCalculator _curvedSpeedByTimeMovementCalculator;
        private readonly bool _shouldDisableCapsuleOverlapObserverWhenIsInactive;

        public PushHandlerController(
            float forceMultiplier,
            float timeMultiplier, float minimumThrowTime, AnimationCurve speedCurve,
            OverlapObserver overlapObserver,
            PushHandlerModel handlerModel, bool shouldDisableCapsuleOverlapObserverWhenIsInactive,
            Action<Vector3> moveAction, Transform movableTransform, bool shouldInvokeOnlyOneExplosionInOneFrame = true)
        {
            _forceMultiplier = forceMultiplier;
            _speedCurve = speedCurve;
            _overlapObserver = overlapObserver;
            _handlerModel = handlerModel;
            _shouldDisableCapsuleOverlapObserverWhenIsInactive = shouldDisableCapsuleOverlapObserverWhenIsInactive;
            _moveAction = moveAction;
            _movableTransform = movableTransform;
            _shouldInvokeOnlyOneExplosionInOneFrame = shouldInvokeOnlyOneExplosionInOneFrame;
            _timeMultiplier = timeMultiplier;
            _minimumThrowTime = minimumThrowTime;
            _curvedSpeedByTimeMovementCalculator = new CurvedSpeedByTimeMovementCalculator();

            if (shouldDisableCapsuleOverlapObserverWhenIsInactive)
                overlapObserver.enabled = false;
        }

        public void TryPush(Vector3 explosionCenter, float forceAtCenter, float forceAtEdge, float radius,
            bool isBlockingExplosion, bool shouldMoveByYAxis)
        {
            var direction = (_movableTransform.transform.position - explosionCenter).normalized;
            var distance = Vector3.Distance(_movableTransform.transform.position, explosionCenter);
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
                _requestedEndPoint = _movableTransform.transform.position +
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
                _overlapObserver.enabled = false;

            _overlapObserver.EventsProvider.Entered -= TryCancelOnCapsuleOverlapEnter;
            _handlerModel.IsPushed = false;
        }

        public async void OnStartMovementAsync()
        {
            _overlapObserver.EventsProvider.Entered -= TryCancelOnCapsuleOverlapEnter;

            Direction = _requestedDirection;

            _curvedSpeedByTimeMovementCalculator.Configure(
                _movableTransform.transform.position,
                _requestedEndPoint, _speedCurve, _requestedTime, out var configured);

            if (!configured)
            {
                Stop();
                return;
            }

            _handlerModel.IsPushed = true;
            _overlapObserver.enabled = true;

            _startExplosionTime = DateTime.Now.Ticks;
            _overlapObserver.EventsProvider.Entered += TryCancelOnCapsuleOverlapEnter;

            await UniTask.WaitForSeconds(_minimumThrowTime);

            if (!_handlerModel.IsPushed || _overlapObserver.CurrentOverlaps.Count == 0 ||
                _overlapObserver.CurrentOverlaps.All(overlap => overlap.isTrigger))
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

            _moveAction.Invoke(Direction * distanceToMoveThisFrame);
        }

        async void TryCancelOnCapsuleOverlapEnter(ActualCollider collider)
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

            if (_handlerModel.IsPushed && _overlapObserver.CurrentOverlaps.Contains(collider))
            {
                Stop();
            }
        }
    }
}