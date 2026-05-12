using System.Collections.Generic;
using FishNet.Transporting;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage.Network.Broadcasts;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Client.Scripts.Network
{
    public class OwnerDamageReceiversSynchronizer : IMatchSharedService
    {
        public OwnerDamageReceiversSynchronizer(
            DamageReceiversRepository damageReceiversesRepository,
            HealthModelsRepository healthModelsesRepository, IOwnerSynchronizersMediator ownerSynchronizersMediator)
        {
            ownerSynchronizersMediator.SubscribeToBroadcast<InitializeDamageReceiversBroadcast>(
                InitializeDamageReceivers);

            return;

            void InitializeDamageReceivers(InitializeDamageReceiversBroadcast broadcast, Channel channel)
            {
                foreach (var data in broadcast.ReceiversData)
                {
                    try
                    {
                        var healthModel =
                            healthModelsesRepository.ValueByKey.GetValueOrDefault(data.ReceiverNetworkObjectId) ??
                            new HealthModel(data.MaxHealthPoints, data.HealthPoints);
                        healthModelsesRepository.TryAdd(data.ReceiverNetworkObjectId, healthModel);
                        damageReceiversesRepository.TryAdd(data.ReceiverNetworkObjectId,
                            new DamageReceiverModel(data.ReceiverNetworkObjectId, healthModel));
                    }
                    catch
                    {
                        Debug.LogError(data.ReceiverNetworkObjectId +
                                       ": is not added to damage receiver! It has been added! QSQUAD");
                    }
                }
            }
        }
    }
}