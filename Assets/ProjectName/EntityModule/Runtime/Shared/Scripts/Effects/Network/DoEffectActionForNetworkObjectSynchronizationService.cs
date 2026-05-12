using System;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Network
{
    public class DoEffectActionForNetworkObjectSynchronizationService : IMatchSharedService
    {
        public event Action<EffectActionData> SentData;

        public void SendEffectActionData(EffectActionData effectActionData)
        {
            SentData?.Invoke(effectActionData);
        }
    }
}