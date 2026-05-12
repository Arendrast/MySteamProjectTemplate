using FishNet.Managing.Client;
using Modules.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.OwnerSynchronizerPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.InventoryModule.Runtime.Shared.Scripts.Network
{
    public class OwnerInventoryItemsChangeSynchronizer : IMatchSharedService
    {
        private InventoryItemsModel OwnerInventoryItemsModel =>
            _clientsInventoryItemsModelsProvider.ModelByNetworkConnection.GetOwners(_clientManager);

        private readonly IClientsInventoryItemsModelsProvider _clientsInventoryItemsModelsProvider;
        private readonly ClientManager _clientManager;

        public OwnerInventoryItemsChangeSynchronizer(
            IClientsInventoryItemsModelsProvider clientsInventoryItemsModelsProvider, ClientManager clientManager, IOwnerSynchronizersMediator ownerSynchronizersMediator)
        {
            _clientsInventoryItemsModelsProvider = clientsInventoryItemsModelsProvider;
            _clientManager = clientManager;
            
            ownerSynchronizersMediator.SubscribeToAction(SubscribeAfterInitialize, Unsubscribe, true);
        }

        private void SubscribeAfterInitialize()
        {
            OwnerInventoryItemsModel.ChangedTargetSlot += SendChangeTargetSlotMessage;
            OwnerInventoryItemsModel.RemovedSlotItem += SendRemoveItemMessage;
            OwnerInventoryItemsModel.AddedSlotItem += SendAddItemMessage;
        }

        private void Unsubscribe()
        {
            if (OwnerInventoryItemsModel != null)
            {
                OwnerInventoryItemsModel.ChangedTargetSlot -= SendChangeTargetSlotMessage;
                OwnerInventoryItemsModel.RemovedSlotItem -= SendRemoveItemMessage;
            }
        }

        private void SendAddItemMessage(int slotIndex)
        {
            _clientManager.Broadcast(new AddSlotItemBroadcastForServer(
                OwnerInventoryItemsModel.SlotsTargetItemModels[slotIndex].Config.Id,
                slotIndex));
        }

        private void SendRemoveItemMessage(int slotIndex, bool onlyOne)
        {
            _clientManager.Broadcast(new RemoveSlotItemBroadcastForServer(slotIndex, onlyOne));
        }

        private void SendChangeTargetSlotMessage(int slotIndex)
        {
            _clientManager.Broadcast(new ChangeTargetSlotBroadcastForServer(slotIndex));
        }
    }
}