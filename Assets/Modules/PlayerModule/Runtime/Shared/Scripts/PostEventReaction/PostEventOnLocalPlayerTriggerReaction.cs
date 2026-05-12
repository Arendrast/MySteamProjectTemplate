using Cysharp.Threading.Tasks;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.PostEventReaction
{
    public class PostEventOnLocalPlayerTriggerReaction : IActionTriggerReaction
    {
        private readonly PostEventOnLocalPlayerTriggerReactionConfig _config;
        private readonly OwnerPlayerFactory _ownerPlayerFactory;

        public PostEventOnLocalPlayerTriggerReaction(PostEventOnLocalPlayerTriggerReactionConfig config,
            OwnerPlayerFactory ownerPlayerFactory)
        {
            _config = config;
            _ownerPlayerFactory = ownerPlayerFactory;
        }

        public async void Invoke()
        {
            if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                    UniTask.WaitWhile(() => _ownerPlayerFactory.OwnerPlayerComponents == null)))
            {
                return;
            }

#if WWISE
            _ownerPlayerFactory.OwnerPlayerComponents.ClientComponents.ViewComponents.SerializableComponents
                .SoundOriginsProviderSerializableComponents.GetMainEventNetworkSender()
                .LocalPostEvent(_config.EventName);
#endif
        }
    }
}