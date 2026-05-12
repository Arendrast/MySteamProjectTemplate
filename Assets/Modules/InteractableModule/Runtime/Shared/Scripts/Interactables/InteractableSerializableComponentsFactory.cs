using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Interactables
{
    public class InteractableSerializableComponentsFactory : IMatchSharedFactory
    {
        private readonly Dictionary<Type, IConcreteInteractablesSerializableComponentsFactory> _concreteInteractableFactoriesByDataType;
        private readonly ServerManager _serverManager;

        public InteractableSerializableComponentsFactory(
            IEnumerable<IConcreteInteractablesSerializableComponentsFactory> concreteInteractableFactories, ServerManager serverManager)
        {
            _serverManager = serverManager;
            _concreteInteractableFactoriesByDataType = concreteInteractableFactories.ToDictionary(factory => factory.GetDataType(), factory => factory);
        }

        public async UniTask<InteractableSerializableComponents> GetCreatedInteractableSerializableComponents(
            IInteractableInitializationData interactableInitializationData, Vector3 position = default,
            Quaternion rotation = default)
        {
            if (!_concreteInteractableFactoriesByDataType.TryGetValue(interactableInitializationData.GetType(), out var concreteFactory))
            {
                return null;
            }
            
            var components =
                await concreteFactory.GetCreatedSerializableComponentsAsync(
                    interactableInitializationData);

            if (components == null)
            {
                return null;
            }
            
            _serverManager.TryCustomSpawn(components.gameObject);
            components.transform.position = position;
            components.transform.rotation = rotation;

            return components;
        }
    }
}