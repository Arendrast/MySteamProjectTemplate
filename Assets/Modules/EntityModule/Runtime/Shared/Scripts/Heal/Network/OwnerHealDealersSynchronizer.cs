using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network.Broadcasts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using MoreLinq.Extensions;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network
{
    public class OwnerHealDealersSynchronizer : IMatchSharedService
    {
        private readonly HealReceiversRepository _healReceiversModelIndexRepository;
        private readonly HealDealersRepository _healDealersModelIndexRepository;

        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;

        public OwnerHealDealersSynchronizer(
            HealDealersRepository healDealersModelIndexRepository,
            HealReceiversRepository healReceiversModelIndexRepository,
            ClientManager clientManager, ServerManager serverManager,
            IOwnerSynchronizersMediator ownerSynchronizersMediator)
        {
            _healDealersModelIndexRepository = healDealersModelIndexRepository;
            _healReceiversModelIndexRepository = healReceiversModelIndexRepository;
            _clientManager = clientManager;
            _serverManager = serverManager;
            
            ownerSynchronizersMediator.SubscribeToAction(SubscribeAfterInitialize, Unsubscribe, false);
        }
        
        private void SubscribeAfterInitialize()
        {
            _healDealersModelIndexRepository.ValueByKey.Values
                .ForEach(model => model.BeforeDealHeal += TrySendDealBeforeDealHealBroadcast);

            _healDealersModelIndexRepository.Added += TrySubscribeToCreatedHealDealers;
            _healDealersModelIndexRepository.Removed += SubscribeToRemoveHealDealers;
        }
            
        private void Unsubscribe()
        {
            _healDealersModelIndexRepository.ValueByKey.Values.ForEach(model =>
                model.BeforeDealHeal -= TrySendDealBeforeDealHealBroadcast);

            _healDealersModelIndexRepository.Added -= TrySubscribeToCreatedHealDealers;
            _healDealersModelIndexRepository.Removed -= SubscribeToRemoveHealDealers;
        }

        private void TrySubscribeToCreatedHealDealers(int networkObjectId, HealDealerModel healDealerModel)
        {
            if (_serverManager.Started || _clientManager.TryGetNetworkObjectById(networkObjectId).IsOwner)
            {
                _healDealersModelIndexRepository.ValueByKey[networkObjectId].BeforeDealHeal +=
                    TrySendDealBeforeDealHealBroadcast;
            }
        }

        private void SubscribeToRemoveHealDealers(int id, HealDealerModel model)
        {
            model.BeforeDealHeal -= TrySendDealBeforeDealHealBroadcast;
        }

        private void TrySendDealBeforeDealHealBroadcast(HealReceiverModel model, DoHealData doHealData)
        {
            if (!_healReceiversModelIndexRepository.KeyByValue.TryGetValue(model, out var id))
            {
                Debug.LogError($"Heal didn't synced. Maybe its destroyed? Heal: {doHealData.Heal}");
                return;
            }

            var broadcast =
                new DealHealBroadcast(id, doHealData);
            
            _clientManager.Broadcast(broadcast);
        }
    }
}