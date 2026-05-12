using System.Collections.Generic;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart
{
    public class StuckResolver : MonoBehaviour
    {
        public float timespan = 1;
        public float minDistance = 0.05f;
        
        private CharacterController _controller;
        private Dictionary<Collider, float> _ignoredColliders = new Dictionary<Collider, float>();

        void Start()
        {
            _controller = GetComponent<CharacterController>();
        }
    
        void Update()
        {
            CleanupIgnoredColliders();
        }
    
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var otherCollider = hit.collider;

            if (_controller == null ||
                otherCollider == null ||
                otherCollider.isTrigger ||
                _ignoredColliders.ContainsKey(otherCollider))
                return;

            var isPenetrating = UnityEngine.Physics.ComputePenetration(
                _controller, 
                transform.position, 
                transform.rotation,
                otherCollider, 
                otherCollider.transform.position, 
                otherCollider.transform.rotation,
                out _, 
                out var distance
            );
            
            if (isPenetrating && distance > minDistance)
            {
                UnityEngine.Physics.IgnoreCollision(_controller, otherCollider, true);
                _ignoredColliders[otherCollider] = Time.time + timespan;
            }
        }
    
        void CleanupIgnoredColliders()
        {
            var toRemove = new List<Collider>();
        
            foreach (var kvp in _ignoredColliders)
            {
                if (Time.time >= kvp.Value)
                {
                    if (kvp.Key != null)
                    {
                        UnityEngine.Physics.IgnoreCollision(_controller, kvp.Key, false);
                    }
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var col in toRemove)
            {
                _ignoredColliders.Remove(col);
            }
        }
    
        void OnDestroy()
        {
            foreach (var kvp in _ignoredColliders)
            {
                if (kvp.Key != null)
                {
                    UnityEngine.Physics.IgnoreCollision(_controller, kvp.Key, false);
                }
            }
        }
    }
}