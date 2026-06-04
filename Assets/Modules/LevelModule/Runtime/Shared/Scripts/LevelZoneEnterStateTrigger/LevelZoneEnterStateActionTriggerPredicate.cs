using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.EntityModule.Runtime.Shared.Scripts.Repositories;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.LevelModule.Runtime.Shared.Scripts.LevelZoneEnterStateTrigger
{
    public class LevelZoneEnterStateActionTriggerPredicate : IActionTriggerPredicate
    {
        public event Action ChangedResult;

        private bool _pastResultValue;

        private readonly LevelZoneEnterStateTriggerConfig _config;
        private readonly LayerMask _playerLayerMask;
        private readonly ClientsConnectionTrackingService _clientsConnectionTrackingService;
        private readonly ClientManager _clientManager;
        private readonly ServerManager _serverManager;
        private readonly EntityComponentsRepository _entityComponentsRepository;

        public LevelZoneEnterStateActionTriggerPredicate(
            LevelZoneEnterStateTriggerConfig config, LayerMask playerLayerMask,
            ClientsConnectionTrackingService clientsConnectionTrackingService, ClientManager clientManager,
            ServerManager serverManager, EntityComponentsRepository entityComponentsRepository)
        {
            _config = config;
            _playerLayerMask = playerLayerMask;
            _clientsConnectionTrackingService = clientsConnectionTrackingService;
            _clientManager = clientManager;
            _serverManager = serverManager;
            _entityComponentsRepository = entityComponentsRepository;

            TryInvokeChangedResultAndSetPastValueWithPerformOverlapCheck();

            _clientsConnectionTrackingService.Connected += TryInvokeChangedResultAndSetPastValue;
            _clientsConnectionTrackingService.Disconnected += TryInvokeChangedResultAndSetPastValue;
            _config.ZoneOverlapObserver.EventsProvider.Entered += TryInvokeChangedResultAndSetPastValueWithCollider;
            _config.ZoneOverlapObserver.EventsProvider.Exited += TryInvokeChangedResultAndSetPastValueWithCollider;

            _entityComponentsRepository.Added += SubscribeToPlayerDeath;
            _entityComponentsRepository.Removed += UnsubscribeFromPlayerDeath;
        }

        private void TryInvokeChangedResultAndSetPastValue(NetworkConnection networkConnection)
        {
            TryInvokeChangedResultAndSetPastValueWithPerformOverlapCheck();
        }

        private void SubscribeToPlayerDeath(EntitySerializableComponents entitySerializableComponents,
            EntityComponents entityComponents)
        {
            entityComponents.HealthModel.DiedWithoutArgs +=
                TryInvokeChangedResultAndSetPastValueWithPerformOverlapCheck;
        }

        private void UnsubscribeFromPlayerDeath(EntitySerializableComponents entitySerializableComponents,
            EntityComponents entityComponents)
        {
            entityComponents.HealthModel.DiedWithoutArgs -=
                TryInvokeChangedResultAndSetPastValueWithPerformOverlapCheck;
        }

        private void TryInvokeChangedResultAndSetPastValueWithPerformOverlapCheck()
        {
            TryInvokeChangedResultAndSetPastValue(true);
        }

        private void TryInvokeChangedResultAndSetPastValue(bool performOverlapCheck)
        {
            var result = GetResult(performOverlapCheck);

            if (result == _pastResultValue) return;

            _pastResultValue = result;
            ChangedResult?.Invoke();
        }


#if TWO_D
        private void TryInvokeChangedResultAndSetPastValueWithCollider(Collider2D collider = null)
        {
            TryInvokeChangedResultAndSetPastValue(false);
        }
#else
        private void TryInvokeChangedResultAndSetPastValueWithCollider(Collider collider = null)
        {
            TryInvokeChangedResultAndSetPastValue(false);
        }
#endif


        public bool GetResult()
        {
            return GetResult(true);
        }

        private bool GetResult(bool performOverlapCheck)
        {
            var requiredPlayersInZoneNumber = _config.ShouldCheckAllPlayersInZone
                ? _serverManager.Started
                    ? _config.CheckForAliveState
                        ? GetAllAlivePlayersCount()
                        : _serverManager.Clients.Count
                    : _config.CheckForAliveState
                        ? GetAllAlivePlayersCount()
                        : _clientManager.Clients.Count
                : _config.RequiredPlayersInZoneNumber;
            
            if (performOverlapCheck)
            {
                _config.ZoneOverlapObserver.PerformOverlapCheck();
            }

            return _config.ZoneOverlapObserver.CurrentOverlaps.Select(collider => collider.gameObject).Distinct()
                .Count(gameObject =>
                    _playerLayerMask.DoesHaveLayer(gameObject.gameObject.layer) &&
                    (!_config.CheckForAliveState ||
                     !_entityComponentsRepository
                         .ValueByKey[gameObject.GetComponentInChildren<EntitySerializableComponents>()].HealthModel
                         .IsDied)) >= requiredPlayersInZoneNumber;

            int GetAllAlivePlayersCount()
            {
                return Mathf.Max(1,
                    _entityComponentsRepository.KeyByValue.Keys.Count(components => !components.HealthModel.IsDied));
            }
        }

        public void Dispose()
        {
            _clientsConnectionTrackingService.Connected -= TryInvokeChangedResultAndSetPastValue;
            _clientsConnectionTrackingService.Disconnected -= TryInvokeChangedResultAndSetPastValue;

            _entityComponentsRepository.Added -= SubscribeToPlayerDeath;
            _entityComponentsRepository.Removed -= UnsubscribeFromPlayerDeath;

            if (_config.ZoneOverlapObserver != null)
            {
                _config.ZoneOverlapObserver.EventsProvider.Entered -= TryInvokeChangedResultAndSetPastValueWithCollider;
                _config.ZoneOverlapObserver.EventsProvider.Exited -= TryInvokeChangedResultAndSetPastValueWithCollider;
                _config.ZoneOverlapObserver.enabled = false;
            }

            ChangedResult = null;
        }
    }
}