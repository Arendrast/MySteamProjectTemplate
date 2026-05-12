using FishNet.Broadcast;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.Network.Broadcasts
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