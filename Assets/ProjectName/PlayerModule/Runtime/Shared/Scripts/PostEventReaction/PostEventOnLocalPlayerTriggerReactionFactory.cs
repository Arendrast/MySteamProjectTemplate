using Cysharp.Threading.Tasks;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.PostEventReaction
{
    public class PostEventOnLocalPlayerTriggerReactionFactory : ConcreteActionTriggerReactionFactory<
        PostEventOnLocalPlayerTriggerReactionConfig>
    {
        private readonly OwnerPlayerFactory _ownerPlayerFactory;

        public PostEventOnLocalPlayerTriggerReactionFactory(OwnerPlayerFactory ownerPlayerFactory)
        {
            _ownerPlayerFactory = ownerPlayerFactory;
        }

        public override UniTask<IActionTriggerReaction> GetConcreteReactionAsync(
            PostEventOnLocalPlayerTriggerReactionConfig config)
        {
            return new UniTask<IActionTriggerReaction>(new PostEventOnLocalPlayerTriggerReaction(config, _ownerPlayerFactory));
        }
    }
}