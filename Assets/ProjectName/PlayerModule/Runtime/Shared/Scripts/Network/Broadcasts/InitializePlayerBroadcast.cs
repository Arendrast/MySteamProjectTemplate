using FishNet.Broadcast;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.SharedStateMachine.States;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct InitializePlayerBroadcast : IBroadcast
    {
        public readonly int GameObjectNetworkObjectId;
        public readonly int ViewNetworkObjectId;
        public readonly int OwnerNetworkConnectionId;
        public readonly int ViewRigNetworkObjectId;
        public readonly InitializeInventoryItemsData InitializeInventoryItemsData;
        public readonly SharedPlayerStateType StateType;
        public readonly InteractableData TargetInteractableData;
        public readonly CharacterType CharacterType;


        public InitializePlayerBroadcast(
            int ownerNetworkConnectionId,
            int gameObjectNetworkObjectID,
            int viewNetworkObjectId,
            int viewRigNetworkObjectId,
            InitializeInventoryItemsData initializeInventoryItemsData,
            SharedPlayerStateType stateType, InteractableData targetInteractableData, CharacterType characterType)
        {
            OwnerNetworkConnectionId = ownerNetworkConnectionId;
            GameObjectNetworkObjectId = gameObjectNetworkObjectID;
            ViewNetworkObjectId = viewNetworkObjectId;
            ViewRigNetworkObjectId = viewRigNetworkObjectId;
            InitializeInventoryItemsData = initializeInventoryItemsData;
            StateType = stateType;
            TargetInteractableData = targetInteractableData;
            CharacterType = characterType;
        }
    }
}