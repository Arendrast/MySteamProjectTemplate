using FishNet.Broadcast;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public struct InitializePlayersBroadcast : IBroadcast
    {
        public readonly InitializePlayerBroadcast[] InitializePlayerMessages;

        public InitializePlayersBroadcast(InitializePlayerBroadcast[] initializePlayerMessages)
        {
            InitializePlayerMessages = initializePlayerMessages;
        }
    }
}