using System;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate
{
    public interface IActionTriggerPredicate : IDisposable
    {
        event Action ChangedResult;

        bool GetResult();
    }
}