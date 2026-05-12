using FishNet.Broadcast;

namespace ProjectName.InteractableModule.Runtime.Shared.Scripts.Network.Broadcasts
{
    public readonly struct TakeAcceptInteractableResultBroadcastForClient : IBroadcast
    {
        public readonly string SerializedInteractionData;
        public readonly bool Successfully;

        public TakeAcceptInteractableResultBroadcastForClient(bool successfully, string serializedInteractionData)
        {
            Successfully = successfully;
            SerializedInteractionData = serializedInteractionData;
        }
    }
}