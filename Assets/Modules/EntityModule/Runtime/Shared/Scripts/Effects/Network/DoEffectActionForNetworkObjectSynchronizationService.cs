using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Network
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