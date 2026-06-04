using System;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Push;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine.InputSystem;
using InputActionType = Modules.SharedModule.Runtime.Shared.Scripts.Input.InputActionType;

namespace Modules.HudModule.Runtime.Scripts.CheatMenu
{
    public class CheatMenuPopupController : IDisposable
    {
        private readonly CheatMenuPopupSerializableComponents _serializableComponents;
        private readonly TimeScaleRepository _timeScaleRepository;
        private readonly IInputService _inputService;

        public CheatMenuPopupController(CheatMenuPopupSerializableComponents serializableComponents,
            DamageReceiverModel damageReceiverModel, PushHandlerModel pushHandlerModel,
            IInputService inputService,
            TimeScaleRepository timeScaleRepository)
        {
            _serializableComponents = serializableComponents;
            _inputService = inputService;
            _timeScaleRepository = timeScaleRepository;

            var damageHandler = new ToZeroDamageHandler();

            serializableComponents.IsImmortalToggle.onValueChanged.AddListener(SetImmortalMode);
            serializableComponents.IsUnpushableToggle.onValueChanged.AddListener(SetIsUnpushable);

            SetSubscribeState(SubscribeState.Subscribe);

            serializableComponents.Popup.Opened += CursorSwitchTools.TryEnableCursor;
            serializableComponents.Popup.Closed += CursorSwitchTools.TryDisableCursor;

            return;

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

        public void Dispose()
        {
            SetSubscribeState(SubscribeState.Unsubscribe);
        }

        private void SetSubscribeState(SubscribeState subscribeState)
        {
            _inputService.SetSubscribeStateToInputAction(InputActionType.CheatMenu, InputActionPhase.Started, TrySetPopupOpenState, subscribeState);
        }

        private void TrySetPopupOpenState(InputAction.CallbackContext context)
        {
            _serializableComponents.Popup.TrySetOpenState();

            _timeScaleRepository.SetTimeScale(_serializableComponents.Popup.IsOpen ? 0 : 1);
        }
    }
}