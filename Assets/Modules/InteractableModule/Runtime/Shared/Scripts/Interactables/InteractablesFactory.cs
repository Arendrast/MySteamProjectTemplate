using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using Modules.EntityModule.Runtime.Shared.Scripts.Push;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public class InteractablesFactory : IMatchSharedFactory
    {
        private readonly InteractablesRepository _interactablesesRepository;
        private readonly ServerManager _serverManager;
        private readonly ConfigsProviderService _configsProviderService;

        private readonly ExplodersFactory _explodersFactory;

        private readonly Dictionary<Type, IConcreteInteractablesFactory> _concreteInteractableFactories;
        private readonly RigidbodyPushablesFactory _rigidbodyPushablesFactory;

        public InteractablesFactory(
            IEnumerable<IConcreteInteractablesFactory> concreteInteractableFactories,
            InteractablesRepository interactablesesRepository,
            ServerManager serverManager,
            ConfigsProviderService configsProviderService,
            RigidbodyPushablesFactory rigidbodyPushablesFactory,
            ExplodersFactory explodersFactory)
        {
            _interactablesesRepository = interactablesesRepository;
            _serverManager = serverManager;
            _configsProviderService = configsProviderService;
            _rigidbodyPushablesFactory = rigidbodyPushablesFactory;
            _explodersFactory = explodersFactory;
            _concreteInteractableFactories =
                concreteInteractableFactories.ToDictionary(factory => factory.GetSerializableComponentsType(),
                    factory => factory);
        }

        public async UniTask<IInteractable> GetCreatedInteractableAsync(
            InteractableSerializableComponents interactableSerializableComponents,
            IInteractableInitializationData interactableInitializationData = null,
            CreateInteractableFlag flag = CreateInteractableFlag.None, bool? canInteract = null)
        {
            if (_interactablesesRepository.ValueByKey.TryGetValue(interactableSerializableComponents,
                    out var interactable))
            {
                return interactable;
            }

            if (!_concreteInteractableFactories.TryGetValue(interactableSerializableComponents.GetType(),
                    out var concreteFactory) ||
                !IsSuitableFlag())
            {
                return null;
            }

            interactable =
                await concreteFactory.GetCreatedInteractableAsync(interactableSerializableComponents,
                    interactableInitializationData);
            
            if (interactable == null) return null;

            if (canInteract.HasValue)
                interactable.CanInteract = canInteract.Value;

            if (interactableSerializableComponents.TryGetComponent<ExplodableSerializableComponents>(
                    out var explodableSerializableComponents))
            {
                _rigidbodyPushablesFactory.TryCreateRigidbodyPushHandler(explodableSerializableComponents);
            }

            if (interactableSerializableComponents.TryGetComponent<ExploderSerializableComponents>(
                    out var exploderSerializableComponents))
            {
                _explodersFactory.TryCreateExploder(exploderSerializableComponents);
            }

            _interactablesesRepository.Add(interactableSerializableComponents, interactable);

            interactableSerializableComponents.gameObject.GetOrAddComponent<EnableDisableObserver>().DisabledGameObject +=
                TryRemoveInteractable;

            _serverManager.TryCustomSpawn(interactableSerializableComponents.gameObject);

            return interactable;

            bool IsSuitableFlag()
            {
                return flag == CreateInteractableFlag.None ||
                       typeof(IClientSyncableInteractable).IsAssignableFrom(concreteFactory.GetInteractableType()) ==
                       (flag == CreateInteractableFlag.OnlySyncable);
            }
        }

        public void UnsubscribeOnDisable(
            InteractableSerializableComponents interactableSerializableComponents,
            IInteractable interactable)
        {
            interactableSerializableComponents.gameObject
                .GetOrAddComponent<EnableDisableObserver>()
                .DisabledGameObject -= TryRemoveInteractable;
        }

        private void TryRemoveInteractable(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<InteractableSerializableComponents>(
                    out var interactableSerializableComponents))
            {
                _interactablesesRepository.RemoveByKey(interactableSerializableComponents);
            }
        }
    }
}