using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class MonoBehaviourObserver : MonoBehaviour
    {
        public event Action Updated, LateUpdated, FixedUpdated, DrawGizmos;
        
        public void Update()
        {
            Updated?.Invoke();
        }

        private void LateUpdate()
        {
            LateUpdated?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdated?.Invoke();
        }

        private void OnDrawGizmos()
        {
            DrawGizmos?.Invoke();
        }
    }
}