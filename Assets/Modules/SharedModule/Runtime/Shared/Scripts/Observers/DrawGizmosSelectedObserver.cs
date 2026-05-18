using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class DrawGizmosSelectedObserver : MonoBehaviour
    {
        public event Action DrawGizmos;
        
        private void OnDrawGizmosSelected()
        {
            DrawGizmos?.Invoke();
        }
    }
}