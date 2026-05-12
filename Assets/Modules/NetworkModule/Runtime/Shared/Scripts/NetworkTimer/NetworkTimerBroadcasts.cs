using FishNet.Broadcast;

namespace Modules.NetworkModule.Runtime.Shared.Scripts.NetworkTimer
{
    public struct TimerStartBroadcast : IBroadcast
    {
        public TimerId TimerId;
    }

    public struct TimerStopBroadcast : IBroadcast
    {
        public TimerId TimerId;
        public bool Completed;
    }

    public struct TimerTickBroadcast : IBroadcast
    {
        public TimerId TimerId;
        public float RemainingTime;
        public float Progress;
    }
}