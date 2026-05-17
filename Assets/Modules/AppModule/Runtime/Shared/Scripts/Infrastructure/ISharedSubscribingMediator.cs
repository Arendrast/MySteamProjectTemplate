using System;

namespace Modules.AppModule.Runtime.Shared.Scripts.Infrastructure
{
    public interface ISharedSubscribingMediator : IDisposable
    {
        void Subscribe();
        void SubscribeAfterInitialize();
    }
}