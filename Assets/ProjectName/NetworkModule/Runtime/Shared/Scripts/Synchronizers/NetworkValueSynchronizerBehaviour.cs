using System;
using ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours;

namespace ProjectName.NetworkModule.Runtime.Shared.Scripts.Synchronizers
{
    public class NetworkValueSynchronizerBehaviour<TValue>
    {
        public event Action<bool, TValue> UpdatedLocally;

        public TValue Value => _isOwnerFunc.Invoke() ? _localClientValue : _getValue.Invoke();

        private TValue _localClientValue;
        
        private readonly Func<TValue> _getValue;
        private readonly Func<bool> _isOwnerFunc;
        private readonly IValueUpdater<TValue> _valueUpdater;

        public NetworkValueSynchronizerBehaviour(Func<bool> isOwnerFunc,
            IValueUpdater<TValue> valueUpdater, Func<TValue> getValue)
        {
            _isOwnerFunc = isOwnerFunc;
            _valueUpdater = valueUpdater;
            _getValue = getValue;
        }

        public void OnStartNetwork()
        {
            UpdatedLocally?.Invoke(_isOwnerFunc.Invoke(), Value);
        }

        public void UpdateValue(TValue value)
        {
            _localClientValue = value;
            _valueUpdater.UpdateValueAsync(value);
            UpdatedLocally?.Invoke(_isOwnerFunc.Invoke(), Value);
        }
    }
}