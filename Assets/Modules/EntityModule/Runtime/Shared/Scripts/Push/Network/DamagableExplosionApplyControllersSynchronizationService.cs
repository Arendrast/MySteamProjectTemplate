using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push.Network
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