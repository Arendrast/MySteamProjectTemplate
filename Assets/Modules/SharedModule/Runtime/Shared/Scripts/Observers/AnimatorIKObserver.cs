using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class AnimatorIKObserver : MonoBehaviour
    {
        public event Action<int> AnimatedIK;

        private void OnAnimatorIK(int layerIndex)
        {
            AnimatedIK?.Invoke(layerIndex);
        }
    }
}