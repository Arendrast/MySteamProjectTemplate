using FishNet.Connection;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Repository;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.SharedModule.Runtime.Server.Scripts
{
    public class LoadedSceneConnectionsRepository : ListRepository<NetworkConnection>, IMatchServerService
    {
        
    }
}