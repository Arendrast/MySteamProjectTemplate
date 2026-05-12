using FishNet.Managing.Server;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Heal;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer
{
    public class OwnerPlayerControllersMediator : IOwnerPlayerComponent
    {
        private readonly OwnerPlayerComponents _ownerPlayerComponents;
        private readonly IInputProvider _inputProvider;
        private readonly EventBus _eventBus;
        private readonly ServerManager _serverManager;
        private readonly Camera _camera;

        public OwnerPlayerControllersMediator(OwnerPlayerComponents ownerPlayerComponents, IInputProvider inputProvider,
            EventBus eventBus, ServerManager serverManager, Camera camera)
        {
            _ownerPlayerComponents = ownerPlayerComponents;
            _inputProvider = inputProvider;
            _eventBus = eventBus;
            _serverManager = serverManager;
            _camera = camera;
        }

        public void Subscribe()
        {
            _ownerPlayerComponents.SerializableComponents.GetOrAddComponent<MonoBehaviourObserver>().Updated +=
                TestDamage;

            SubscribeToControllersEvents(_ownerPlayerComponents);

            return;

            void TestDamage()
            {
                if (_inputProvider.IsActionTriggered(InputActionType.Test2))
                {
                    _ownerPlayerComponents.ClientComponents.EntityComponents.DamageDealerModel.DoDamage(
                        _ownerPlayerComponents.ClientComponents.EntityComponents.DamageReceiverModel, 
                        new DoDamageData(10, DamageOrigin.Test));
                }
                else if (_inputProvider.IsActionTriggered(InputActionType.CameraShake))
                {
                    _ownerPlayerComponents.ClientComponents.EntityComponents.HealDealerModel.DoHeal(
                        _ownerPlayerComponents.ClientComponents.EntityComponents.HealReceiverModel, 
                        new DoHealData(10, HealOrigin.MyselfKit, overridedMaxHealPoints: 250));
                }
            }
        }

        private void SubscribeToControllersEvents(OwnerPlayerComponents ownerPlayerComponents)
        {
        }
    }
}