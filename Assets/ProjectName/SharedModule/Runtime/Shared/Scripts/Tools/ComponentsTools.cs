using FishNet.Object;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class ComponentsTools
    {
        public static int? GetNetworkObjectId(this Component component)
        {
            return GetNetworkObjectId(component.gameObject);
        }

        public static int? GetNetworkObjectId(this GameObject gm)
        {
            if (gm.TryGetComponent<NetworkObject>(out var networkObject))
            {
                return networkObject.ObjectId;
            }

            return null;
        }

        public static int? GetNetworkObjectOwnerId(this Component component)
        {
            return GetNetworkObjectOwnerId(component.gameObject);
        }

        public static int? GetNetworkObjectOwnerId(this GameObject gm)
        {
            if (gm.TryGetComponent<NetworkObject>(out var networkObject))
            {
                return networkObject.Owner.ClientId;
            }

            return null;
        }
    }
}