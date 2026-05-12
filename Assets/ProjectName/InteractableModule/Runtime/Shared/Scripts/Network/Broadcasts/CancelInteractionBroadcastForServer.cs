using FishNet.Broadcast;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public readonly struct CancelInteractionBroadcastForServer : IBroadcast
    {
        public readonly int NetworkObjectId;

        public CancelInteractionBroadcastForServer(int networkObjectId)
        {
            NetworkObjectId = networkObjectId;
        }
    }
}