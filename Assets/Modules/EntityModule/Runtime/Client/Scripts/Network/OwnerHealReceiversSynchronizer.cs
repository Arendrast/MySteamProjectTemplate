using System.Collections.Generic;
using FishNet.Transporting;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal.Network.Broadcasts;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Client.Scripts.Network
{
    public class OwnerHealReceiversSynchronizer : IMatchSharedService
    {
        public OwnerHealReceiversSynchronizer(
            HealReceiversRepository healReceiversesRepository,
            HealthModelsRepository healthModelsesRepository, IOwnerSynchronizersMediator ownerSynchronizersMediator)
        {
            ownerSynchronizersMediator.SubscribeToBroadcast<InitializeHealReceiversBroadcast>(InitializeHealReceivers);

            return;

            void InitializeHealReceivers(InitializeHealReceiversBroadcast broadcast, Channel channel)
            {
                foreach (var data in broadcast.ReceiversData)
                {
                    try
                    {
                        var healthModel =
                            healthModelsesRepository.ValueByKey.GetValueOrDefault(data.ReceiverNetworkObjectId) ??
                            new HealthModel(data.MaxHealthPoints, data.HealthPoints);
                        healthModelsesRepository.TryAdd(data.ReceiverNetworkObjectId, healthModel);
                        healReceiversesRepository.TryAdd(data.ReceiverNetworkObjectId,
                            new HealReceiverModel(healthModel));
                    }
                    catch
                    {
                        Debug.LogError(data.ReceiverNetworkObjectId +
                                       ": is not added to heal receiver! It has been added! QSQUAD");
                    }
                }
            }
        }
    }
}