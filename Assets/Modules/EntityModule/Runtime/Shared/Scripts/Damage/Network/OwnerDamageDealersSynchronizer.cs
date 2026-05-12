using System;
using System.Collections.Generic;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using MoreLinq.Extensions;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network
{
    public class OwnerDamageDealersSynchronizer : IMatchSharedService
    {
        private readonly DamageReceiversRepository _damageReceiversModelIndexRepository;
        private readonly DamageDealersRepository _damageDealersModelIndexRepository;

        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;

        private readonly Dictionary<DamageDealerModel, Action<DamageReceiverModel, DoDamageData>> _handlers = new();

        public OwnerDamageDealersSynchronizer(
            DamageDealersRepository damageDealersModelIndexRepository,
            DamageReceiversRepository damageReceiversModelIndexRepository,
            ClientManager clientManager, ServerManager serverManager,
            IOwnerSynchronizersMediator ownerSynchronizersMediator)
        {
            _damageDealersModelIndexRepository = damageDealersModelIndexRepository;
            _damageReceiversModelIndexRepository = damageReceiversModelIndexRepository;
            _clientManager = clientManager;
            _serverManager = serverManager;

            ownerSynchronizersMediator.SubscribeToAction(SubscribeAfterInitialize, Unsubscribe, false);
        }

        private void SubscribeAfterInitialize()
        {
            _damageDealersModelIndexRepository.ValueByKey
                .ForEach(pair => SubscribeToCreatedDamageDealers(pair.Key, pair.Value));

            _damageDealersModelIndexRepository.Added += SubscribeToCreatedDamageDealers;
            _damageDealersModelIndexRepository.Removed += TryUnsubscribeFromRemoveDamageDealers;
        }

        private void Unsubscribe()
        {
            _damageDealersModelIndexRepository.ValueByKey.ForEach(pair =>
                TryUnsubscribeFromRemoveDamageDealers(pair.Key, pair.Value));

            _damageDealersModelIndexRepository.Added -= SubscribeToCreatedDamageDealers;
            _damageDealersModelIndexRepository.Removed -= TryUnsubscribeFromRemoveDamageDealers;
        }

        private void SubscribeToCreatedDamageDealers(int networkObjectId, DamageDealerModel damageDealerModel)
        {
            Action<DamageReceiverModel, DoDamageData> handler = (damageReceiverModel, doDamageData) =>
                TrySendDealBeforeDealDamageBroadcast(damageReceiverModel, doDamageData, networkObjectId);
            _handlers.Add(damageDealerModel, handler);
            damageDealerModel.BeforeDealDamage += handler;
        }

        private void TryUnsubscribeFromRemoveDamageDealers(int id, DamageDealerModel model)
        {
            if (_handlers.TryGetValue(model, out var handler))
            {
                model.BeforeDealDamage -= handler;
                _handlers.Remove(model);
            }
        }

        private void TrySendDealBeforeDealDamageBroadcast(DamageReceiverModel model, DoDamageData doDamageData,
            int dealerId)
        {
            var dealerNetworkObject = _clientManager.TryGetNetworkObjectById(dealerId);

            if ((!_serverManager.Started && !dealerNetworkObject.IsOwner) ||
                (_serverManager.Started && !dealerNetworkObject.IsOwner && dealerNetworkObject.Owner != null))
            {
                Debug.Log(model.Id + ": " + dealerNetworkObject.name);
                return;
            }

            if (!_damageReceiversModelIndexRepository.KeyByValue.TryGetValue(model, out var id))
            {
                Debug.LogError($"Damage didn't synced. Maybe its destroyed? Damage: {doDamageData.Damage}");
                return;
            }

            var broadcast =
                new DealDamageBroadcast(id, doDamageData);

            _clientManager.Broadcast(broadcast);
        }
    }
}