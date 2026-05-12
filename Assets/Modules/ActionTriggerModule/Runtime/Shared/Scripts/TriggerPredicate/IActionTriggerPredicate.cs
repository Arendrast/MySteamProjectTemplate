using System;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate
{
    public interface IActionTriggerPredicate : IDisposable
    {
        event Action ChangedResult;

        bool GetResult();
    }
}