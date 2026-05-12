using System.Collections.Generic;
using FishNet.Connection;

namespace Modules.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IClientsTargetInteractableDataRepositoriesProvider
    {
        IReadOnlyDictionary<NetworkConnection, TargetInteractableDataRepository> Repositories { get; }
    }
}