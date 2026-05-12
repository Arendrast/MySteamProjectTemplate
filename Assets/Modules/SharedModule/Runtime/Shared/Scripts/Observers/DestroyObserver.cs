using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class DestroyObserver : MonoBehaviour
    {
        public event Action Destroyed;
        public event Action<GameObject> DestroyedGameObject;
        private void OnDestroy()
        {
            Destroyed?.Invoke();
            DestroyedGameObject?.Invoke(gameObject);
        }
    }
}