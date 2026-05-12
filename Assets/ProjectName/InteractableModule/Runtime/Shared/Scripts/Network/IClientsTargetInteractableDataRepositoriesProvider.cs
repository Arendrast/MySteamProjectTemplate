using System.Collections.Generic;
using FishNet.Connection;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network
{
    public interface IClientsTargetInteractableDataRepositoriesProvider
    {
        IReadOnlyDictionary<NetworkConnection, TargetInteractableDataRepository> Repositories { get; }
    }
}