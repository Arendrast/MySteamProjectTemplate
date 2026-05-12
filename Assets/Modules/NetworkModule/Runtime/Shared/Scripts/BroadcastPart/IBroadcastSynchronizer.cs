namespace Modules.NetworkModule.Runtime.Shared.Scripts.BroadcastPart
{
    public interface IBroadcastSynchronizer
    {
        void SubscribeToBroadcast();
        void UnsubscribeFromBroadcast();
    }
}