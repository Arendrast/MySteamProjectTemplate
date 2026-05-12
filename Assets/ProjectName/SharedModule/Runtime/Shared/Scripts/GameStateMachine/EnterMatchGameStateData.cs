namespace ProjectName.SharedModule.Runtime.Shared.Scripts.GameStateMachine
{
    public struct EnterMatchGameStateData : IGameStateEnterData
    {
        public readonly bool IsHost;
        public readonly string HostSteamId;
        public readonly int TargetLevelIndex;
        public readonly int TargetSafeZoneNumber;

        public EnterMatchGameStateData(bool isHost, string hostSteamId, int targetLevelIndex, int targetSafeZoneNumber)
        {
            IsHost = isHost;
            HostSteamId = hostSteamId;
            TargetLevelIndex = targetLevelIndex;
            TargetSafeZoneNumber = targetSafeZoneNumber;
        }
    }
}