using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Client.Scripts.AnimationEventSystem
{
    public class AnimationEventsObserver : MonoBehaviour
    {
        public event Action<string> InvokedStringEvent; 
        public event Action<int> InvokedIntEvent; 
        public event Action<float> InvokedFloatEvent; 
        public event Action InvokedEvent; 
        
        public void InvokeEvent()
        {
            InvokedEvent?.Invoke();
        }
        
        public void InvokeEvent(string @string)
        {
            InvokedStringEvent?.Invoke(@string);
        }
        
        public void InvokeEvent(int @int)
        {
            InvokedIntEvent?.Invoke(@int);
        }
        
        public void InvokeEvent(float @float)
        {
            InvokedFloatEvent?.Invoke(@float);
        }
    }
}