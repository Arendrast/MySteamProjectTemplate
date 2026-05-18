using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Heal;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using UnityEngine.InputSystem;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public class PlayerTestInputHandler : IPlayerInputHandler
    {
        private readonly IInputService _inputService;
        private readonly ClientPlayerComponents _clientComponents;

        public PlayerTestInputHandler(IInputService inputService, ClientPlayerComponents clientComponents)
        {
            _inputService = inputService;
            _clientComponents = clientComponents;
        }

        public void SetSubscribeState(SubscribeState subscribeState)
        {
            _inputService.SetSubscribeStateToInputActionGroup(InputActionGroupType.Test, InputActionPhase.Started, Test, 2, subscribeState);
        }

        public PlayerInputHandlerType GetInputHandlerType()
        {
            return PlayerInputHandlerType.Test;
        }

        private void Test(InputAction.CallbackContext callbackContext, int testIndex)
        {
            if (testIndex == 0)
            {
                _clientComponents.EntityComponents.DamageDealerModel.DoDamage(
                    _clientComponents.EntityComponents.DamageReceiverModel, 
                    new DoDamageData(10, DamageOrigin.Test));
            }
            else if (testIndex == 1)
            {
                _clientComponents.EntityComponents.HealDealerModel.DoHeal(
                    _clientComponents.EntityComponents.HealReceiverModel, 
                    new DoHealData(10, HealOrigin.None, overridedMaxHealPoints: 250));
            }
        }
    }
}