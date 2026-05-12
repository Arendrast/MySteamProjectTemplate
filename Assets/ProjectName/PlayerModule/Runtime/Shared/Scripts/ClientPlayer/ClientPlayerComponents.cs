using System;
using FishNet.Connection;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;
using ProjectName.InventoryModule.Runtime.Shared.Scripts;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
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