using Modules.SharedModule.Runtime.Shared.Scripts.Services;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
{
    public class OwnerPlayerRepository : IMatchSharedService
    {
        public OwnerPlayerComponents OwnerPlayerComponents { get; set; }
    }
}