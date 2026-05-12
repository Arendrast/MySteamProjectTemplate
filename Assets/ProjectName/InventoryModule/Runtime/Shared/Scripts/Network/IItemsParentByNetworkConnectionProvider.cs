using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine;

namespace ProjectName.InventoryModule.Runtime.Shared.Scripts.Network
{
    public interface IItemsParentByNetworkConnectionProvider 
    {
        IReadOnlyDictionary<NetworkConnection, Transform> Parents { get; }
    }
}