using System;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using Action = System.Action;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.InputHandlers
{
    public sealed class PlayerInputHandler
    {
        public readonly EnableStateProvider EnableStateProvider = new();

        private readonly Func<bool> _inputConditionFunc;
        private readonly Action _returnedTrueCondition, _returnedFalseCondition;

        public PlayerInputHandler(Func<bool> inputConditionFunc, PressActionInputHandler pressActionInputHandler)
        {
            _returnedTrueCondition = pressActionInputHandler.TryEnter;
            _returnedFalseCondition = pressActionInputHandler.TryExit;
            _inputConditionFunc = inputConditionFunc;
        }
        
        public PlayerInputHandler(Func<bool> inputConditionFunc, Action returnedTrueCondition,
            Action returnedFalseCondition = null)
        {
            _returnedTrueCondition = returnedTrueCondition;
            _returnedFalseCondition = returnedFalseCondition;
            _inputConditionFunc = inputConditionFunc;
        }

        public void InvokeActions()
        {
            if (!EnableStateProvider.IsEnable)
                return;

            if (_inputConditionFunc.Invoke())
                _returnedTrueCondition?.Invoke();
            else
                _returnedFalseCondition?.Invoke();
        }
    }
}