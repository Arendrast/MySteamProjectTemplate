using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class EnableDisableObserver : MonoBehaviour
    {
        public event Action Enabled, Disabled;
        public event Action<GameObject> EnabledGameObject, DisabledGameObject;
        
        private void OnEnable()
        {
            Enabled?.Invoke();
            EnabledGameObject?.Invoke(gameObject);
        }

        private void OnDisable()
        {
            Disabled?.Invoke();
            DisabledGameObject?.Invoke(gameObject);
        }
    }
}