using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
{
    public class OwnerPlayerRepository : IMatchSharedService
    {
        public OwnerPlayerComponents OwnerPlayerComponents { get; set; }
    }
}