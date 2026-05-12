using System;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class MonoBehaviourObserver : MonoBehaviour
    {
        public event Action Updated, LateUpdated, FixedUpdated, DrawedGizmos;
        public event Action<int> AnimatorIKed;
        
        public void Update()
        {
            Updated?.Invoke();
        }

        private void LateUpdate()
        {
            LateUpdated?.Invoke();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            AnimatorIKed?.Invoke(layerIndex);
        }

        private void FixedUpdate()
        {
            FixedUpdated?.Invoke();
        }

        private void OnDrawGizmos()
        {
            DrawedGizmos?.Invoke();
        }
    }
}