using System;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using UniRx;
using UnityEngine;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.NetworkTimer
{
    public interface INetworkTimerService : IDisposable
    {
        event Action<TimerId> TimerStarted;
        event Action<TimerId> TimerCompleted;
        event Action<TimerId> TimerStopped;
        event Action<TickData> TimerTicked;

        public void Enable();

        void StartTimer(out TimerId? timerId, float duration, string tag, bool shouldInvokeTickedEvent = false);
        void StopTimer(TimerId timerId);
        bool IsTimerRunning(TimerId timerId);
    }

    public class NetworkTimerService : INetworkTimerService, IMatchSharedService
    {
        private class TimerData
        {
            public TimerId Id;
            public float Duration;
            public float RemainingTime;
            public bool IsRunning;
            public float StartTime;
            public bool ShouldInvokeTickedEvent;
        }

        public event Action<TimerId> TimerStarted;
        public event Action<TimerId> TimerCompleted;
        public event Action<TimerId> TimerStopped;
        public event Action<TickData> TimerTicked;

        private CompositeDisposable _disposables = new();
        private readonly Dictionary<TimerId, TimerData> _timers = new();
        private readonly NetworkManager _networkManager;
        private readonly ServerManager _serverManager;
        private readonly ClientManager _clientManager;

        public NetworkTimerService(NetworkManager networkManager, ServerManager serverManager,
            ClientManager clientManager)
        {
            _networkManager = networkManager;
            _serverManager = serverManager;
            _clientManager = clientManager;
        }

        public void Enable()
        {
            if (_networkManager.IsClientStarted)
            {
                _clientManager.RegisterBroadcast<TimerStartBroadcast>(OnTimerStartBroadcast);
                _clientManager.RegisterBroadcast<TimerStopBroadcast>(OnTimerStopBroadcast);
                _clientManager.RegisterBroadcast<TimerTickBroadcast>(OnTimerTickBroadcast);
            }

            if (_networkManager.IsServerStarted)
            {
                _disposables = new CompositeDisposable();
                
                Observable.EveryUpdate()
                    .Subscribe(_ => UpdateTimers())
                    .AddTo(_disposables);
            }
        }

        private void UpdateTimers()
        {
            if (!_networkManager.IsServerStarted)
            {
                return;
            }

            var completedTimers = new List<TimerId>();
            
            foreach (var timerPair in _timers)
            {
                var timer = timerPair.Value;
                
                if (!timer.IsRunning) continue;

                timer.RemainingTime -= Time.deltaTime;
                
                if (timer.ShouldInvokeTickedEvent)
                    SendTimerTick(timer);

                if (timer.RemainingTime > 0f) continue;

                timer.RemainingTime = 0f;
                completedTimers.Add(timerPair.Key);
            }

            foreach (var timerId in completedTimers)
            {
                CompleteTimer(timerId);
            }
        }

        public void StartTimer(out TimerId? timerId, float duration, string tag, bool shouldInvokeTickedEvent = false)
        {
            if (!_networkManager.IsServerStarted)
            {
                Debug.LogWarning("Cannot start timer: Server not started");
                timerId = null;
                return;
            }

            timerId = new TimerId($"{tag}_{Guid.NewGuid().ToString().Substring(0, 8)}");

            var timerData = new TimerData
            {
                Id = timerId.Value,
                Duration = duration,
                RemainingTime = duration,
                IsRunning = true,
                StartTime = Time.time,
                ShouldInvokeTickedEvent = shouldInvokeTickedEvent
            };

            _timers[timerId.Value] = timerData;

            var broadcast = new TimerStartBroadcast
            {
                TimerId = timerId.Value
            };

            _serverManager.Broadcast(broadcast);

            if (!_networkManager.IsClientStarted)
            {
                TimerStarted?.Invoke(timerId.Value);
            }
            SendTimerTick(timerData);
        }

        public void StopTimer(TimerId timerId)
        {
            if (!_networkManager.IsServerStarted || !_timers.TryGetValue(timerId, out var timer))
            {
                return;
            }

            timer.IsRunning = false;

            var broadcast = new TimerStopBroadcast
            {
                TimerId = timerId,
                Completed = false
            };
            _serverManager.Broadcast(broadcast);

            if (!_networkManager.IsClientStarted)
            {
                TimerStopped?.Invoke(timerId);
            }

            _timers.Remove(timerId);
        }

        public bool IsTimerRunning(TimerId timerId)
        {
            return _timers.ContainsKey(timerId) && _timers[timerId].IsRunning;
        }

        private void CompleteTimer(TimerId timerId)
        {
            if (!_timers.TryGetValue(timerId, out var timer))
            {
                return;
            }

            timer.IsRunning = false;

            var broadcast = new TimerStopBroadcast
            {
                TimerId = timerId,
                Completed = true
            };
            _serverManager.Broadcast(broadcast);

            if (_serverManager.Started)
            {
                TimerCompleted?.Invoke(timerId);
            }

            _timers.Remove(timerId);
        }

        private void SendTimerTick(TimerData timer)
        {
            var progress = 1f - (timer.RemainingTime / timer.Duration);

            var broadcast = new TimerTickBroadcast
            {
                TimerId = timer.Id,
                RemainingTime = timer.RemainingTime,
                Progress = progress
            };

            _serverManager.Broadcast(broadcast);

            if (!_networkManager.IsClientStarted)
            {
                TimerTicked?.Invoke(new TickData(timer.Id, timer.RemainingTime, progress));
            }
        }

        private void OnTimerStartBroadcast(TimerStartBroadcast broadcast, Channel channel)
        {
            TimerStarted?.Invoke(broadcast.TimerId);
        }

        private void OnTimerStopBroadcast(TimerStopBroadcast broadcast, Channel channel)
        {
            if (broadcast.Completed)
            {
                TimerCompleted?.Invoke(broadcast.TimerId);
            }
            else
            {
                TimerStopped?.Invoke(broadcast.TimerId);
            }
        }

        private void OnTimerTickBroadcast(TimerTickBroadcast broadcast, Channel channel)
        {
            TimerTicked?.Invoke(new TickData(broadcast.TimerId, broadcast.RemainingTime, broadcast.Progress));
        }

        public void Dispose()
        {
            _timers.Clear();
            _disposables?.Dispose();

            if (!_networkManager.IsClientStarted) return;
            _clientManager.UnregisterBroadcast<TimerStartBroadcast>(OnTimerStartBroadcast);
            _clientManager.UnregisterBroadcast<TimerStopBroadcast>(OnTimerStopBroadcast);
            _clientManager.UnregisterBroadcast<TimerTickBroadcast>(OnTimerTickBroadcast);
        }
    }
}