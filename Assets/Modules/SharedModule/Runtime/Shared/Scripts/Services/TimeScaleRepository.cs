using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Services
{
    public class TimeScaleRepository : IPersistentService
    {
        public float TimeScale { get; private set; } = 1;
        
        public void SetTimeScale(float timeScale)
        { 
            TimeScale = Mathf.Clamp(timeScale, 0, 100);
        }

        public void MakeTimeScaleZero()
        {
            SetTimeScale(0);
        }
        
        public void MakeTimeScaleDefault()
        {
            SetTimeScale(1);
        }

        public bool IsTimeScaleZero() => TimeScale == 0;
    }
}