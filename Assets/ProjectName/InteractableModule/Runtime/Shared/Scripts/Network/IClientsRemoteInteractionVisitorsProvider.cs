using System.Collections.Generic;
using FishNet.Connection;
using ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IClientsRemoteInteractionVisitorsProvider
    {
        IReadOnlyDictionary<NetworkConnection, INotOwnerInteractionVisitor> NotOwnersInteractionVisitors { get; }
    }
}