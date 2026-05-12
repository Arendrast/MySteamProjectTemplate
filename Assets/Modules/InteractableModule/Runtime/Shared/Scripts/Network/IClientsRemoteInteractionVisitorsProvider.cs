using System.Collections.Generic;
using FishNet.Connection;
using Modules.InteractableModule.Runtime.Shared.Scripts.Network.Visitors;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IClientsRemoteInteractionVisitorsProvider
    {
        IReadOnlyDictionary<NetworkConnection, INotOwnerInteractionVisitor> NotOwnersInteractionVisitors { get; }
    }
}