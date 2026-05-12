using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.LevelModule.Runtime.Shared.Scripts
{
    public class LevelZoneFactoryRepository : IMatchSharedService
    {
        public LevelZoneFactory LevelZoneFactory { get; set; }
    }
}