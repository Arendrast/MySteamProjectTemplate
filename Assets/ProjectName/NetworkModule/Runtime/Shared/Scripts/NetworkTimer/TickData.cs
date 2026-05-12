namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.NetworkTimer
{
    public class TickData
    {
        public readonly TimerId TimerId;
        public readonly float RemainingTime;
        public readonly float Progress;

        public TickData(TimerId timerId, float remainingTime, float progress)
        {
            TimerId = timerId;
            RemainingTime = remainingTime;
            Progress = progress;
        }
    }
}