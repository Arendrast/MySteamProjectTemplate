using System.Collections.Generic;
using FishNet.Connection;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class NetworkConnectionEqualityComparer : IEqualityComparer<NetworkConnection>
    {
        public bool Equals(NetworkConnection x, NetworkConnection y)
        {
            return x is { IsValid: true } && y is { IsValid: true } ? x.Equals(y) : x.CustomEquals(y);
        }

        public int GetHashCode(NetworkConnection obj)
        {
            return obj is { IsValid: true} ? obj.GetHashCode() : 0;
        }
    }
}