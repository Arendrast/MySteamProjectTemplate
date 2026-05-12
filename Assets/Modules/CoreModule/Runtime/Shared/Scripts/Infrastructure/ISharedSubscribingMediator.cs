using System;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public interface ISharedSubscribingMediator : IDisposable
    {
        void Subscribe();
        void SubscribeAfterInitialize();
    }
}