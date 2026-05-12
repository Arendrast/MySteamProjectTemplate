using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters
{
    public class NetworkCountersSynchronizerBehaviour : NetworkBehaviour
    {
        public IReadOnlyDictionary<CounterType, int> Counters => _counters.ToDictionary(pair => pair.Key, pair => pair.Value); 
        
        public event Action<CounterType, int> UpdatedCounter;

        private readonly SyncDictionary<CounterType, int> _counters = new SyncDictionary<CounterType, int>();
        
        public override void OnStartNetwork()
        {
            if (IsServerInitialized)
            {
                foreach (var counter in CollectionTools.ParseEnumToList<CounterType>())
                {
                    _counters.Add(counter, 0);
                }
            }

            _counters.OnChange += OnChangeDictionary;
        }

        private void OnChangeDictionary(SyncDictionaryOperation op, CounterType key, int value, bool asServer)
        {
            if (op == SyncDictionaryOperation.Add || op == SyncDictionaryOperation.Set)
            {
                UpdatedCounter?.Invoke(key, value);
            }
        }

        public void TryUpdateValue(CounterType counterType, int value)
        {
            if (!IsServerInitialized)
                return;
            
            _counters[counterType] = value;
        }
    }
}