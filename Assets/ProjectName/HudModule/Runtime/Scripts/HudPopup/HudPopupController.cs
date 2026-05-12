using System;
using System.Threading;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Entity;
using ProjectName.HudModule.Runtime.Scripts.CheatMenu;
using ProjectName.HudModule.Runtime.Scripts.GameHint;
using ProjectName.HudModule.Runtime.Scripts.LowHealPoints;
using ProjectName.InventoryModule.Runtime.Shared.Scripts.UI;
using ProjectName.ItemModule.Runtime.Shared.Scripts.View;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using ProjectName.SharedModule.Runtime.Shared.Scripts.EventBusSystem.Events;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectName.HudModule.Runtime.Scripts.HudPopup
{
    public class HudPopupController : IDisposable
    {
        public Transform Root => _serializableComponents.Popup.transform;

        private CancellationTokenSource _cancellationTokenSource;

        private readonly HealthBarController _healthBarController;
        private readonly HudPopupSerializableComponents _serializableComponents;
        private readonly EventBus _eventBus;
        private readonly LowHealPointsPopupController _lowHealPointsPopupController;

        public HudPopupController(HudPopupSerializableComponents serializableComponents,
            ItemsViewConfig itemsViewConfig,
            EventBus eventBus,
            IInputProvider inputProvider,
            TimeScaleRepository timeScaleRepository, Vignette mainVignette,
            OwnerPlayerComponents ownerPlayerComponents,
            NetworkCountersSynchronizerBehaviour countersSynchronizerBehaviour)
        {
            _serializableComponents = serializableComponents;
            _eventBus = eventBus;

            eventBus.Subscribe<SetOpenStateHudPopupEvent>(SetOpenState);

            new InventoryItemsWindowController(
                serializableComponents.InventoryItemsWindowSerializableComponents,
                ownerPlayerComponents.ClientComponents.InventoryItemsModel,
                itemsViewConfig);

            new CheatMenuPopupController(serializableComponents.CheatMenuPopupSerializableComponents,
                ownerPlayerComponents.ClientComponents.EntityComponents.DamageReceiverModel,
                ownerPlayerComponents.PushHandlerModel,
                inputProvider, timeScaleRepository);

            new InteractTextController(serializableComponents.InteractText,
                ownerPlayerComponents.InteractionController);

            _lowHealPointsPopupController = new LowHealPointsPopupController(
                _serializableComponents.LowHealPointsPopupSerializableComponents,
                ownerPlayerComponents.ClientComponents.EntityComponents.HealthModel, mainVignette);

            new GameHintWindowController(countersSynchronizerBehaviour,
                _serializableComponents.GameHintsWindowSerializableComponents);
        }

        private void SetOpenState(SetOpenStateHudPopupEvent @event)
        {
            _serializableComponents.Popup.TrySetOpenState(@event.IsOpen);
        }

        public void Disable()
        {
            _serializableComponents.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<SetOpenStateHudPopupEvent>(SetOpenState);
            _healthBarController?.Dispose();
            _lowHealPointsPopupController?.Dispose();
        }
    }
}