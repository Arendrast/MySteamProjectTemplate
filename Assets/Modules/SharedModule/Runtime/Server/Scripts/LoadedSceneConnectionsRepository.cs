using FishNet.Connection;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.Repository;

namespace Modules.SharedModule.Runtime.Server.Scripts
{
    public class LoadedSceneConnectionsRepository : ListRepository<NetworkConnection>, IMatchServerService
    {
        
    }
}