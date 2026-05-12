using System.Collections.Generic;
using System.Linq;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.SubscribingMediators;
using MoreLinq;

namespace Modules.CoreModule.Runtime.Server.Scripts.Infrastructure.Services
{
    public class ServerSubscribingMediator : IMatchServerService, ISharedSubscribingMediator
    {
        private readonly IList<IServerSubscribingMediator> _subscribingMediators;
        private readonly IList<ISubscribingMediatorAfterInitialize> _subscribingMediatorsAfterInitialize;

        public ServerSubscribingMediator(
            IEnumerable<IServerSubscribingMediator> subscribingMediators)
        {
            _subscribingMediators = subscribingMediators.ToList();
            _subscribingMediatorsAfterInitialize =
                _subscribingMediators.OfType<ISubscribingMediatorAfterInitialize>().ToList();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void Subscribe()
        {
            _subscribingMediators.ForEach(mediator => mediator.Subscribe());
        }

        public void SubscribeAfterInitialize()
        {
            _subscribingMediatorsAfterInitialize.ForEach(mediator => mediator.SubscribeAfterInitialize());
        }

        public void Unsubscribe()
        {
            _subscribingMediators.ForEach(mediator => mediator.Unsubscribe());
        }
    }
}