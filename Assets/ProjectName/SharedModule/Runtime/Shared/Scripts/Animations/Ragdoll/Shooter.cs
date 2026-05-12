using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Animations.Ragdoll
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField, Range(0, 10000)] private float _force;

        private Camera _camera;
        private RagdollController _ragdollController;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Construct(RagdollController ragdollController)
        {
            _ragdollController = ragdollController;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 forceDirection = (hit.point - _camera.transform.position).normalized;
                    forceDirection.y = 0;

                    _ragdollController.Hit(forceDirection * _force, hit.point);
                }
            }
        }
    }
}