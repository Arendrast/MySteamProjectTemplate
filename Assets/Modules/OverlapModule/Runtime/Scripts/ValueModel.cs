using System;
using System.Diagnostics;

namespace Modules.OverlapModule.Runtime.Scripts
{
    public class ValueModel<T>
    {
        public T Value
        {
            get
            {
#if UNITY_EDITOR
                return _hasOverridenValue ? _overridedValue : ConfigValue;
#endif

                return _overridedValue;
            }
        }

        public T ConfigValue => _configValueFunc.Invoke();

        private readonly Func<T> _configValueFunc;

#if UNITY_EDITOR
        private bool _hasOverridenValue;
#endif

        private T _overridedValue;

        public ValueModel(Func<T> configValueFunc)
        {
            _configValueFunc = configValueFunc;
            ResetToConfigValue();
        }

        public void ResetToConfigValue()
        {
            SetValue(ConfigValue);
            SetOverriddenFlag(false);
        }

        public void SetValue(T value)
        {
            _overridedValue = value;
            SetOverriddenFlag(true);
        }

        [Conditional("UNITY_EDITOR")]
        private void SetOverriddenFlag(bool state)
        {
            _hasOverridenValue = state;
        }
    }
}