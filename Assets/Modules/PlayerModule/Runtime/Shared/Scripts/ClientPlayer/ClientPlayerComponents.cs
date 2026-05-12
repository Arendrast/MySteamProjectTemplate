using System;
using FishNet.Connection;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using Modules.InventoryModule.Runtime.Shared.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View;
using Modules.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
{
    public class ClientPlayerComponents : IDisposable
    {
        public readonly InventoryItemsModel InventoryItemsModel;
        public readonly NetworkConnection NetworkConnection;

        public readonly ClientPlayerSerializableComponents SerializableComponents;
        public readonly PlayerViewComponents ViewComponents;
        public readonly EntityComponents EntityComponents;
        public readonly INotOwnerInteractionVisitor NotOwnerInteractionVisitor;
        public readonly FiniteStateMachineModel<IPlayerSharedState> StateMachine;
        public readonly TargetInteractableDataRepository TargetInteractableDataRepository;
        public readonly IsFirstEnterPlayerSharedStateRepository IsFirstEnterPlayerSharedStateRepository;

        public ClientPlayerComponents(ClientPlayerSerializableComponents serializableComponents,
            EntityComponents entityComponents,
            InventoryItemsModel inventoryItemsModel,
            INotOwnerInteractionVisitor notOwnerInteractionVisitor,
            PlayerViewComponents viewComponents, 
            FiniteStateMachineModel<IPlayerSharedState> stateMachine,
            TargetInteractableDataRepository targetInteractableDataRepository,
            IsFirstEnterPlayerSharedStateRepository isFirstEnterPlayerSharedStateRepository,
            NetworkConnection networkConnection)
        {
            SerializableComponents = serializableComponents;
            EntityComponents = entityComponents;
            InventoryItemsModel = inventoryItemsModel;
            NotOwnerInteractionVisitor = notOwnerInteractionVisitor;
            ViewComponents = viewComponents;
            StateMachine = stateMachine;
            TargetInteractableDataRepository = targetInteractableDataRepository;
            IsFirstEnterPlayerSharedStateRepository = isFirstEnterPlayerSharedStateRepository;
            NetworkConnection = networkConnection;
        }

        public void Dispose()
        {
            
        }
    }
}