using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using ProjectName.SharedModule.Runtime.Client.Scripts.Localization;

namespace ProjectName.HudModule.Runtime.Scripts.GameHint
{
    public class GameHintWindowController
    {
        private readonly GameHintWindowSerializableComponents _gameHintWindowSerializableComponents;

        public GameHintWindowController(NetworkCountersSynchronizerBehaviour countersSynchronizerBehaviour,
            GameHintWindowSerializableComponents gameHintWindowSerializableComponents)
        {
            _gameHintWindowSerializableComponents = gameHintWindowSerializableComponents;

            countersSynchronizerBehaviour.UpdatedCounter += TryUpdateTextAsync;
            UpdateTextAsync(countersSynchronizerBehaviour.Counters[CounterType.GameHint]);
        }

        private void TryUpdateTextAsync(CounterType counterType, int hintCounter)
        {
            if (counterType == CounterType.GameHint)
                UpdateTextAsync(hintCounter);
        }

        private async void UpdateTextAsync(int hintCounter)
        {
            _gameHintWindowSerializableComponents.HintText.text =
                await LocalizationTools.GetLocalizedStringAsync(LocalizationTablesHolder.GameHints,
                    "Hint " + hintCounter);
        }
    }
}