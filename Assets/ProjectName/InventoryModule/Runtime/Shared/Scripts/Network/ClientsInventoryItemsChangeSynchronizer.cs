using FishNet.Managing.Client;
using FishNet.Transporting;
using ProjectName.InventoryModule.Runtime.Shared.Scripts.Network.Broadcasts;
using ProjectName.ItemModule.Runtime.Shared.Scripts.Logic;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.ClientsSynchronizerPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network
{
    public class ClientsInventoryItemsChangeSynchronizer : IMatchSharedService
    {
        private readonly IClientsInventoryItemsModelsProvider _clientsInventoryItemsModelsProvider;
        private readonly ClientManager _clientManager;
        private readonly ItemsFactory _itemsFactory;
        private readonly IItemsParentByNetworkConnectionProvider _provider;

        public ClientsInventoryItemsChangeSynchronizer(
            IClientsInventoryItemsModelsProvider clientsInventoryItemsModelsProvider, ClientManager clientManager,
            ItemsFactory itemsFactory, IClientsSynchronizersMediator mediator, IItemsParentByNetworkConnectionProvider provider)
        {
            _clientsInventoryItemsModelsProvider = clientsInventoryItemsModelsProvider;
            _clientManager = clientManager;
            _itemsFactory = itemsFactory;
            _provider = provider;

            mediator.SubscribeToBroadcast<ChangeTargetSlotBroadcastForClient>(UpdateAnotherClientTargetSlotAsync);
            mediator.SubscribeToBroadcast<RemoveSlotItemBroadcastForClient>(RemoveSlotItemForClientAsync);
            mediator.SubscribeToBroadcast<AddSlotItemBroadcastForClient>(AddSlotItemForClientAsync);
        }

        private async void AddSlotItemForClientAsync(AddSlotItemBroadcastForClient broadcast, Channel channel)
        {
            var connection = _clientManager.GetNetworkConnectionByClientId(broadcast.FromNetworkConnectionId);
            var inventoryModel = await _clientsInventoryItemsModelsProvider.TryGetModel(connection);
            var parent = _provider.Parents[connection];

            var itemModel = await _itemsFactory.GetItemModelAsync(broadcast.ItemId, parent);
            
            inventoryModel.TryAddItemToSlot(itemModel, broadcast.SlotIndex);
        }

        private async void RemoveSlotItemForClientAsync(RemoveSlotItemBroadcastForClient broadcast, Channel channel)
        {
            var connection = _clientManager.GetNetworkConnectionByClientId(broadcast.FromNetworkConnectionId);
            var inventoryModel = await _clientsInventoryItemsModelsProvider.TryGetModel(connection);

            inventoryModel.TryRemoveItemsFromSlot(broadcast.SlotIndex, false, broadcast.OnlyOne);
        }

        private async void UpdateAnotherClientTargetSlotAsync(ChangeTargetSlotBroadcastForClient broadcast,
            Channel channel)
        {
            var connection = _clientManager.GetNetworkConnectionByClientId(broadcast.FromNetworkConnectionId);
            var inventoryModel = await _clientsInventoryItemsModelsProvider.TryGetModel(connection);

            inventoryModel.StartSetTargetSlot(broadcast.SlotIndex, true);
        }
    }
}