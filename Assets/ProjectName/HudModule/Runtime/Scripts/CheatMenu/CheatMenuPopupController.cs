using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Push;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.HudModule.Runtime.Scripts.CheatMenu
{
    public class CheatMenuPopupController
    {
        public CheatMenuPopupController(CheatMenuPopupSerializableComponents serializableComponents,
            DamageReceiverModel damageReceiverModel, CharacterControllerPushHandlerModel pushHandlerModel,
            IInputProvider inputProvider,
            TimeScaleRepository timeScaleRepository)
        {
            var damageHandler = new ToZeroDamageHandler();

            serializableComponents.IsImmortalToggle.onValueChanged.AddListener(SetImmortalMode);
            serializableComponents.IsUnpushableToggle.onValueChanged.AddListener(SetIsUnpushable);

            serializableComponents.GetOrAddComponent<MonoBehaviourObserver>().Updated += TrySetPopupOpenState;

            serializableComponents.Popup.Opened += CursorSwitchTools.TryEnableCursor;
            serializableComponents.Popup.Closed += CursorSwitchTools.TryDisableCursor;

            return;

            void TrySetPopupOpenState()
            {
                if (!inputProvider.IsActionTriggered(InputActionType.CheatMenu))
                    return;

                serializableComponents.Popup.TrySetOpenState();

                timeScaleRepository.SetTimeScale(serializableComponents.Popup.IsOpen ? 0 : 1);
            }

            void SetIsUnpushable(bool value)
            {
                pushHandlerModel.CanPush = !value;
            }

            void SetImmortalMode(bool value)
            {
                if (value)
                    damageReceiverModel
                        .TryAddDamageHandler(damageHandler);
                else
                    damageReceiverModel
                        .TryRemoveDamageHandler(damageHandler);
            }
        }
    }
}