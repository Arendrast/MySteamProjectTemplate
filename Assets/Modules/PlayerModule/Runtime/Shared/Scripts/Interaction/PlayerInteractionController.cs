using System;
using System.Collections.Generic;
using System.Linq;
using Modules.InteractableModule.Runtime.Shared.Scripts.Interactables;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Interaction
{
    public class PlayerInteractionController : IInteractionObserver, IOwnerInteractionController, IOwnerPlayerComponent
    {
        private bool DoesWaitForInteractWithTargetInteractable => _targetInteractable != null;
        public event Action<IInteractable, IAdditionalInteractionData> StartedInteraction;
        public event Action<IInteractable> CancelledApprovedInteraction, UnsuccessfullyInteracted;
        public event Action<IInteractable, InteractableSerializableComponents> DetectedInteractable;
        public event Action NotDetectedInteractable;

        private IInteractable _targetInteractable;

        private readonly ActiveInputHandlersTypesRepository _activeInputHandlersTypesRepository;
        private readonly Camera _camera;
        private readonly IOwnerInteractionVisitor _visitor;
        private readonly PhysicsLayersConfig _physicsLayersConfig;
        private readonly InteractablesRepository _interactablesesRepository;
        private readonly OwnerPlayerSerializableComponents _ownerPlayerComponents;

        public PlayerInteractionController(IOwnerInteractionVisitor visitor, Camera camera,
            PhysicsLayersConfig physicsLayersConfig,
            InteractablesRepository interactablesesRepository,
            ActiveInputHandlersTypesRepository activeInputHandlersTypesRepository,
            OwnerPlayerSerializableComponents ownerPlayerComponents)
        {
            _visitor = visitor;
            _camera = camera;
            _physicsLayersConfig = physicsLayersConfig;
            _interactablesesRepository = interactablesesRepository;
            _activeInputHandlersTypesRepository = activeInputHandlersTypesRepository;
            _ownerPlayerComponents = ownerPlayerComponents;
        }

        public void TryInteractWithTargetInteractable(IFromServerInteractionData interactionData)
        {
            if (!_activeInputHandlersTypesRepository.ActiveInputHandlerTypes.Contains(
                    PlayerInputHandlerType.Interaction))
            {
                CancelledApprovedInteraction?.Invoke(_targetInteractable);
                CancelInteractionWithTargetInteractable();
                return;
            }

            ((IClientSyncableInteractable)_targetInteractable).Accept(_visitor, interactionData);
            AfterInteract();
        }

        public void CancelInteractionWithTargetInteractable()
        {
            _targetInteractable = null; 
        }

        public async void TryInteractAsync()
        {
            if (DoesWaitForInteractWithTargetInteractable)
                return;

            var ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (!Physics.Raycast(ray.origin, ray.direction,
                    out var hit,
                    _ownerPlayerComponents.MaxInteractionDistance,
                    _physicsLayersConfig.LayerMaskByLayerGroup[PhysicsLayerGroup.Interactable]))
            {
                NotDetectedInteractable?.Invoke();
                return;
            }

            var interactableSerializableComponents = hit.collider.GetComponentInParentsByPredicate<InteractableSerializableComponents>();

            var interactable = interactableSerializableComponents == null
                ? null
                : _interactablesesRepository.ValueByKey.GetValueOrDefault(interactableSerializableComponents);
            
            DetectedInteractable?.Invoke(interactable, interactableSerializableComponents);

            if (interactable == null || !interactable.CanInteract)
            {
                return;
            }

            var additionalInteractionDataContainer = new DataContainer<IAdditionalInteractionData>();

            if (interactable is IClientSyncableInteractable syncableInteractable)
            {
                var canLocalAccept =
                    await syncableInteractable.CanLocalAcceptAsync(_visitor, additionalInteractionDataContainer);

                if (!canLocalAccept)
                {
                    UnsuccessfullyInteracted?.Invoke(interactable);
                    return;
                }

                _targetInteractable = interactable;
            }
            else if (interactable is ILocalInteractable localInteractable)
            {
                _targetInteractable = interactable;
                localInteractable.Accept(_visitor);
                AfterInteract();
            }
            
            StartedInteraction?.Invoke(interactable, additionalInteractionDataContainer.Data);
        }

        private void AfterInteract()
        {
            if (_targetInteractable is IRemovableInteractable removableInteractable &&
                removableInteractable.ShouldBeRemoved())
                _interactablesesRepository.RemoveByValue(_targetInteractable);

            _targetInteractable = null;
        }
    }
}