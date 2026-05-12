using System;
using System.Threading;
using UnityEngine;
using Timer = ProjectName.SharedModule.Runtime.Shared.Scripts.QoL.Timer;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Pulsation
{
    public class PulsationController
    {
        public event Action<float> Pulsated;

        private float _remainingValueForAddForPulsation;
        private bool _shouldIncreasePulsationValue;
        private float _vignetteStartValue;
        private bool _oneTime;
        
        private readonly PulsationConfig _config;
        private readonly Timer _pulsationTimer;
        
        public PulsationController(PulsationConfig config, CancellationToken cancellationToken)
        {
            _config = config;

            _pulsationTimer = new Timer(cancellationToken);
            _pulsationTimer.Updated += Pulsate;
            _pulsationTimer.Ended += RestartTimer;
        }

        public void StartPulsate(bool oneTime)
        {
            _shouldIncreasePulsationValue = true;
            _remainingValueForAddForPulsation = _config.PulsationValue;
            _pulsationTimer.TryStartCountingTime(_config.PulsationFrequency / 2);
            _oneTime = oneTime;
        }
        
        public void TryStartPulsate(bool onlyIfDoesntPulsate, bool oneTime)
        {
            if (onlyIfDoesntPulsate && _pulsationTimer.IsCounting())
                return;
            
            StartPulsate(oneTime);
        }

        public void Stop()
        {
            _pulsationTimer.TryStopCountingTime();
        }
        
        private void RestartTimer()
        {
            if (!_shouldIncreasePulsationValue && _oneTime)
                return;
            
            _shouldIncreasePulsationValue = !_shouldIncreasePulsationValue;
            _remainingValueForAddForPulsation = _shouldIncreasePulsationValue
                ? _config.PulsationValue
                : -_config.PulsationValue;
            _pulsationTimer.TryStartCountingTime(_config.PulsationFrequency / 2);
        }

        private void Pulsate(float remainingTime)
        {
            var valueForAdd = _config.PulsationValue * Time.deltaTime /
                _pulsationTimer.AppointedTime * (_shouldIncreasePulsationValue ? 1 : -1);

            if (Mathf.Abs(valueForAdd) > Mathf.Abs(_remainingValueForAddForPulsation))
            {
                valueForAdd = _remainingValueForAddForPulsation;
            }

            _remainingValueForAddForPulsation -= valueForAdd;
            Pulsated?.Invoke(valueForAdd);
        }
    }
}