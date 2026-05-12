using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers
{
    public class ColliderObserver : MonoBehaviour
    {
        public IReadOnlyList<Collider> EnteredColliders
        {
            get
            {
                _enteredColliders.RemoveAll(collider => !collider || !collider.gameObject.activeSelf);
                return _enteredColliders;
            }
        }
        
        public IReadOnlyList<Collision> EnteredCollisions
        {
            get
            {
                _enteredCollisions.RemoveAll(collision => !collision.collider || !collision.gameObject.activeSelf);
                return _enteredCollisions;
            }
        }

        public event Action<Collider> EnteredCollider, ExitedCollider, StayedCollider;
        public event Action<Collision> EnteredCollision, ExitedCollision, StayedCollision;

        [SerializeField] private List<Collider> _enteredColliders = new();
        
        private readonly List<Collision> _enteredCollisions = new ();

        private void OnDisable()
        {
            foreach (var collision in _enteredCollisions.ToList())
            {
                OnCollisionExit(collision);
            }
            
            foreach (var collider in _enteredColliders.ToList())
            {
                OnTriggerExit(collider);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            //Debug.Log(other.gameObject.name);
            OnEnter(other.collider, OnEnterCollision);

            return;
            
            void OnEnterCollision()
            {
                _enteredCollisions.Add(other);
                EnteredCollision?.Invoke(other);   
            }
        }

        private void OnCollisionStay(Collision other)
        {
            OnStay(other.collider, OnStayCollision);
            //Debug.Log(other.gameObject.name);

            return;
            
            void OnStayCollision()
            {
                StayedCollision?.Invoke(other);   
            }
        }

        private void OnCollisionExit(Collision other)
        {
            OnExit(other.collider, OnExitCollision);

            return;
            
            void OnExitCollision()
            {
                _enteredCollisions.Remove(other);
                ExitedCollision?.Invoke(other);   
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            OnEnter(hit.collider);
            OnStay(hit.collider);
        }

        private void OnStay(Collider other, Action stayed = null)
        {
            if (!_enteredColliders.Contains(other))
                return;

            StayedCollider?.Invoke(other);
            stayed?.Invoke();
        }

        private void OnEnter(Collider other, Action entered = null)
        {
            if (_enteredColliders.Contains(other))
                return;

            _enteredColliders.Add(other);
            EnteredCollider?.Invoke(other);
            entered?.Invoke();
        }

        private void OnExit(Collider other, Action exited = null)
        {
            if (!_enteredColliders.Remove(other))
            {
                return;;
            }
            
            ExitedCollider?.Invoke(other);
            exited?.Invoke();
        }
    }
}