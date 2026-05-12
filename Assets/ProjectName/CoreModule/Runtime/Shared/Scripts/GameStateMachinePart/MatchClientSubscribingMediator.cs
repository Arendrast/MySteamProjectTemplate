using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Client.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.SubscribingMediators;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart
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