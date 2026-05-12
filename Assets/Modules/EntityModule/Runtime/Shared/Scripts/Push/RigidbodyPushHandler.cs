using System;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Push
{
    public class RigidbodyPushHandler : IPushable
    {
        public event Action<bool> Pushed;
        
        private readonly Rigidbody _rigidbody;

        public RigidbodyPushHandler(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void TryPush(Vector3 explosionCenter, float forceAtCenter, float forceAtEdge, float radius, bool isBlockingExplosion, bool shouldMoveByYAxis)
        {
            var direction = (_rigidbody.transform.position - explosionCenter).normalized;
            var distance = Vector3.Distance(_rigidbody.transform.position, explosionCenter);
            var resultForce = distance / radius;
            
            TryPush(resultForce, shouldMoveByYAxis ? direction : direction.WithY(0), isBlockingExplosion);
        }

        public void TryPush(float moveDistance, Vector3 direction, bool isBlockingExplosion)
        {
            _rigidbody.AddForce(direction * moveDistance, ForceMode.Impulse);
            
            Pushed?.Invoke(isBlockingExplosion);
        }
    }
}