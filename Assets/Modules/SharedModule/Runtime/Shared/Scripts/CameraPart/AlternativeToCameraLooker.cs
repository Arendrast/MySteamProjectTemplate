using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class AlternativeToCameraLooker : MonoBehaviour
    {
        [field: SerializeField] public Vector3 Offset { get; private set; }
        
        private UnityEngine.Camera _mainCamera;

        private void Start()
        {
            if (!_mainCamera)
                Construct(UnityEngine.Camera.main);
        }

        private void Update()
        {
            if (!_mainCamera)
            {
                //Debug.LogWarning("Main camera is not found!");
                return;
            }

            LookAtCamera();
        }

        public void Construct(UnityEngine.Camera mainCamera) => _mainCamera = mainCamera;

        private void LookAtCamera()
        {
            var directionToCamera = _mainCamera.transform.position - transform.position;

            directionToCamera.y = 0;

            if (directionToCamera != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToCamera);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Offset);
            }
        }
    }
}