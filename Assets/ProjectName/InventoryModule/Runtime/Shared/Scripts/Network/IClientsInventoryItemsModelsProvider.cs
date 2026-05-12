using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Connection;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network
{
    public interface IClientsInventoryItemsModelsProvider
    {
        IReadOnlyDictionary<NetworkConnection, InventoryItemsModel> ModelByNetworkConnection { get; }
        public UniTask<InventoryItemsModel> TryGetModel(NetworkConnection networkConnection);
    }
}