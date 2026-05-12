using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class Timer : IReadOnlyTimer
    {
        public float AppointedTime => _appointedTime;
        public float RemainingTime { get; private set; }

        public float PastTime => AppointedTime - RemainingTime;
        public bool IsEnded => AppointedTime > 0 && RemainingTime == 0;
        public bool IsPause { get; private set; }

        #region Events

        public event Action InterruptedIncompleted, Ended;
        public event Action<float> Updated, Ticked, UpdatedOnStartNextSecond, Started, ChangedAppointedTime;

        #endregion

        private float _appointedTime;
        private bool _isCountDown;
        private int _lastSecondUpdateTime;

        private readonly CancellationToken _destroyCancellationToken;
        private CancellationTokenSource _sharedTokenSource;


        public Timer(CancellationToken destroyCancellationToken = default, float appointedTime = 0)
        {
            _destroyCancellationToken = destroyCancellationToken;
            _sharedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_destroyCancellationToken);
            TryUpdateAppointedTime(appointedTime);
        }

        public void ClearEnded() => Ended = null;

        #region Start

        public static async UniTask TryStartCountingTime(float time, Action ended, bool isTimeScaled = true,
            CancellationToken token = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), isTimeScaled, PlayerLoopTiming.Update, token);
            ended?.Invoke();
        }

        public void Configure(float appointedTime, float remainingTime)
        {
            TryUpdateAppointedTime(appointedTime);
            TryUpdateRemainingTime(remainingTime);
        }

        public void ConfigureAndTryStartingCountingTime(float appointedTime, float remainingTime)
        {
            Configure(appointedTime, remainingTime);
            TryStartCountingTime(appointedTime, false);
        }

        public void TryStartCountingTime(float appointedTime, bool shouldStartFromBeginning = true,
            bool isScaled = true)
        {
            if (_isCountDown)
                return;

            TryUpdateAppointedTime(appointedTime, shouldStartFromBeginning);
            TryCountDownAsync(isScaled: isScaled, shouldStartFromBeginning: shouldStartFromBeginning).Forget();
        }

        public void TryStartCountingTime(bool shouldStartFromBeginning = true, bool isScaled = true)
            => TryStartCountingTime(_appointedTime, shouldStartFromBeginning, isScaled);

        #endregion

        #region Set

        public void Reset() => RemainingTime = _appointedTime;

        public void TryUpdateAppointedTime(float appointedTime, bool shouldStartFromBeginning = true)
        {
            appointedTime = Mathf.Max(0, appointedTime);

            if (ShouldStopCountingImmediately(appointedTime))
            {
                TryStopingCountingImmediately();
                return;
            }

            _appointedTime = appointedTime;
            TryAppointAppointedTimeForRemainingTime(shouldStartFromBeginning);
            ChangedAppointedTime?.Invoke(_appointedTime);
        }

        public void TryUpdateRemainingTime(float time)
        {
            time = Mathf.Max(0, time);

            if (time > _appointedTime) return;

            RemainingTime = time;

            if (ShouldStopCountingImmediately(time))
                TryStopingCountingImmediately();
        }

        public void Add(float time) =>
            _appointedTime += time;

        public void IncreaseRemainingTime(float addTime) =>
            RemainingTime += addTime;

        #endregion

        #region Pause

        public void SetPauseState(bool isPause)
        {
            IsPause = isPause;
        }

        private void MakeIsPauseFalse() => SetPauseState(false);

        #endregion

        #region Stop

        public bool IsCounting() => _isCountDown;

        public void TryStopCountingTime()
        {
            if (!_isCountDown)
                return;

            CancelAndRecreateSharedTokenSource();
            _isCountDown = false;
            InterruptedIncompleted?.Invoke();
        }

        private void CancelAndRecreateSharedTokenSource()
        {
            _sharedTokenSource.Cancel();
            _sharedTokenSource.Dispose();
            _sharedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_destroyCancellationToken);
        }

        private void TryStopingCountingImmediately()
        {
            if (_isCountDown)
                CancelAndRecreateSharedTokenSource();
        }

        #endregion

        private async UniTaskVoid TryCountDownAsync(bool isScaled, bool shouldStartFromBeginning = true)
        {
            if (_isCountDown)
                return;

            TryAppointAppointedTimeForRemainingTime(shouldStartFromBeginning);
            
            _isCountDown = true;

            Started?.Invoke(RemainingTime);
            Updated?.Invoke(RemainingTime);
            UpdatedOnStartNextSecond?.Invoke(RemainingTime);
            _lastSecondUpdateTime = -1;
            
            while (RemainingTime > 0)
            {
                if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                    UniTask.DelayFrame(1, cancellationToken: _sharedTokenSource.Token, cancelImmediately: true)))
                {
                    MakeIsPauseFalse();
                    _isCountDown = false;
                    InterruptedIncompleted?.Invoke();
                    return;
                }

                if (IsPause)
                {
                    continue;
                }

                var deltaTime = isScaled ? Time.deltaTime : Time.unscaledDeltaTime;
                RemainingTime = Mathf.Max(RemainingTime - deltaTime, 0);

                if (_lastSecondUpdateTime != (int)RemainingTime)
                {
                    UpdatedOnStartNextSecond?.Invoke(RemainingTime);
                    _lastSecondUpdateTime = (int)RemainingTime;
                }

                Updated?.Invoke(RemainingTime);
                Ticked?.Invoke(RemainingTime);
            }
            
            OnEnd();
        }

        private void TryAppointAppointedTimeForRemainingTime(bool shouldStartFromBeginning = true)
        {
            if (shouldStartFromBeginning || RemainingTime > _appointedTime)
                RemainingTime = _appointedTime;
        }

        private bool ShouldStopCountingImmediately(float appointedOrRemainingTime)
            => appointedOrRemainingTime <= float.Epsilon && _isCountDown;

        private void OnEnd()
        {
            MakeIsPauseFalse();
            RemainingTime = 0;
            _isCountDown = false;
            UpdatedOnStartNextSecond?.Invoke(0);
            Updated?.Invoke(0);
            Ended?.Invoke();
        }
    }
}