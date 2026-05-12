using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class TimeTools
    {
        private const int OneTickInSeconds = 10000000;
        public static long FromSecondsToTicks(this float seconds) => (long) (seconds * OneTickInSeconds);
        public static float FromTicksToSeconds(this long ticks) => ((float)ticks / OneTickInSeconds);
        public static TimeSpan Milliseconds(this float time)
        {
            return TimeSpan.FromMilliseconds(time);
        }

        public static long GetPastTimeInTicks(this long time)
        {
            return DateTime.Now.Ticks - time;
        }

        public static float GetPastTimeInSeconds(this long time) => GetPastTimeInTicks(time).FromTicksToSeconds();
        
        public static TimeSpan Seconds(this float time)
        {
            return TimeSpan.FromSeconds(time);
        }
        
        public static TimeSpan Minutes(this float time)
        {
            return TimeSpan.FromMinutes(time);
        }
    }
}