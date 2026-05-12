using System;
using System.Threading;
using Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using Modules.EntityModule.Runtime.Shared.Scripts.Entity;
using Modules.HudModule.Runtime.Scripts.CheatMenu;
using Modules.HudModule.Runtime.Scripts.GameHint;
using Modules.HudModule.Runtime.Scripts.LowHealPoints;
using Modules.InventoryModule.Runtime.Shared.Scripts.UI;
using Modules.ItemModule.Runtime.Shared.Scripts.View;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.EventBusSystem.Events;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Modules.HudModule.Runtime.Scripts.HudPopup
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