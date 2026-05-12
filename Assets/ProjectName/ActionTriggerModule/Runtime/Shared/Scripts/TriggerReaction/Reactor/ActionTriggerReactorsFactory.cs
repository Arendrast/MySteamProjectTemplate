using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using MoreLinq;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactor
{
    public class ActionTriggerReactorsFactory : IMatchSharedFactory, IDisposable
    {
        private readonly List<IActionTriggerPredicate> _predicates = new List<IActionTriggerPredicate>();

        private readonly ActionTriggerPredicatesFactory _predicatesFactory;
        private readonly ActionTriggerReactionsFactory _reactionsFactory;
        private readonly ServerManager _serverManager;

        public ActionTriggerReactorsFactory(ActionTriggerPredicatesFactory predicatesFactory,
            ServerManager serverManager, ActionTriggerReactionsFactory reactionsFactory)
        {
            _predicatesFactory = predicatesFactory;
            _serverManager = serverManager;
            _reactionsFactory = reactionsFactory;
        }

        public async UniTask TryInitializeReactorAsync(ActionTriggerReactorSerializableComponents component)
        {
            if (component.TriggerConfigs.Count == 0 || (component.ServerAuthoritative && !_serverManager.Started))
                return;

            var reactionsTask = component.TriggerReactionConfigs.Select(async config =>
                await _reactionsFactory.GetCreatedActionTriggerReactionAsync(config));

            var reactions = await UniTask.WhenAll(reactionsTask);

            var predicatesTask = component.TriggerConfigs.Select(async config =>
                await _predicatesFactory.GetCreatedActionTriggerPredicateAsync(config));

            var predicates = await UniTask.WhenAll(predicatesTask);

            predicates.ForEach(predicate => predicate.ChangedResult += TryReactAndDispose);

            _predicates.AddRange(predicates);

            TryReactAndDispose();

            return;

            void TryReactAndDispose()
            {
                if (predicates.Any(predicate => !predicate.GetResult()))
                    return;

                reactions.ForEach(async (reaction, i) =>
                {
                    if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(UniTask.WaitForSeconds(
                            component.TriggerReactionConfigs[i].DelayBeforeReaction,
                            cancellationToken: component.destroyCancellationToken)))
                    {
                        return;
                    }
                    
                    reaction.Invoke();
                });

                if (!component.ShouldDisposeAfterReaction) return;

                predicates.ForEach(predicate => predicate.Dispose());
            }
        }

        public void Dispose()
        {
            _predicates.ForEach(predicate => predicate.Dispose());
            _predicates.Clear();
        }
    }
}