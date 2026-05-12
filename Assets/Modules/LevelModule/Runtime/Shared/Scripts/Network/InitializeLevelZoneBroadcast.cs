using FishNet.Broadcast;

namespace Modules.LevelModule.Runtime.Shared.Scripts.Network
{
    public readonly struct InitializeLevelZoneBroadcast : IBroadcast
    {
        public readonly int EnvironmentNetworkObjectId;

        public InitializeLevelZoneBroadcast(int environmentNetworkObjectId)
        {
            EnvironmentNetworkObjectId = environmentNetworkObjectId;
        }
    }
}