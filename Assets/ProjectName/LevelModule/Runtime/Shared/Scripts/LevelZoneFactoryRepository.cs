using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    public class LevelZoneFactoryRepository : IMatchSharedService
    {
        public LevelZoneFactory LevelZoneFactory { get; set; }
    }
}