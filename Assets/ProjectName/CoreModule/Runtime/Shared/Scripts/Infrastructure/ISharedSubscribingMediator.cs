using System;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public interface ISharedSubscribingMediator : IDisposable
    {
        void Subscribe();
        void SubscribeAfterInitialize();
    }
}