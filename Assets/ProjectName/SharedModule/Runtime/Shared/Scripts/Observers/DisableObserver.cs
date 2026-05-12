using System;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class DisableObserver : MonoBehaviour
    {
        public event Action Disabled;
        public event Action<GameObject> DisabledGameObject;
        private void OnDisable()
        {
            Disabled?.Invoke();
            DisabledGameObject?.Invoke(gameObject);
        }
    }
}