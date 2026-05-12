using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.QoL
{
    public class ToMovementForwardRotator : MonoBehaviour
    {
        private Vector3 _lastPoint;

        private void Awake()
        {
            _lastPoint = transform.position;
        }

        private void Update()
        {
            if (_lastPoint == transform.position)
            {
                return;
            }

            MoveToDirection();
        }

        private void MoveToDirection()
        {
            transform.forward = (transform.position - _lastPoint).normalized;
            _lastPoint = transform.position;
        }
    }
}