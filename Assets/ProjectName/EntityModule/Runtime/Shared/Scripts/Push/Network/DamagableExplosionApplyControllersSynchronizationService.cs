using System;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public class DamagableExplosionApplyControllersSynchronizationService : IMatchSharedService
    {
        public event Action<ExplosionData, int> SentData;
        
        public void Send(ExplosionData data, int damageDealerId)
        {
            SentData?.Invoke(data, damageDealerId);
        }
    }
}