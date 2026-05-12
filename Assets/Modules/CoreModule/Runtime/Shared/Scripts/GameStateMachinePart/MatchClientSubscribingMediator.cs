using System.Collections.Generic;
using System.Linq;
using Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Client.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Shared.Scripts.SubscribingMediators;
using MoreLinq.Extensions;

namespace Modules.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public class MatchClientSubscribingMediator : IMatchClientService, ISharedSubscribingMediator
    {
        private readonly IList<IClientSubscribingMediator> _subscribingMediators;
        private readonly IList<ISubscribingMediatorAfterInitialize> _subscribingMediatorsAfterInitialize;

        public MatchClientSubscribingMediator(IEnumerable<IClientSubscribingMediator> subscribingMediators)
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

        public void SubscribeAfterInitialize() =>
            _subscribingMediatorsAfterInitialize.ForEach(mediator => mediator.SubscribeAfterInitialize());

        public void Unsubscribe()
        {
            _subscribingMediators.ForEach(mediator => mediator.Unsubscribe());
        }
    }
}