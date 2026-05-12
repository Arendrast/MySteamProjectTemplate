using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.QoL
{
    public interface IReadOnlyTimer
    {
        float AppointedTime { get; }
        float RemainingTime { get; }
        bool IsCounting();
        event Action<float> Updated, UpdatedOnStartNextSecond, Started;
        event Action Ended;
    }
}