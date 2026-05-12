using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class FPSCameraController
    {
        private float _xRotation;
        private float _yRotation;

        private bool _shouldRotateByLookInputX;
        private float _maxHorizontalAngle;
        private float _minHorizontalAngle;
        private float _maxVerticalAngle;
        private float _minVerticalAngle;
        private bool _isEnabledRotateCameraByLookInput;

        private readonly FPSCameraSerializableComponents _serializableComponents;
        private readonly IInputProvider _inputProvider;
        private readonly CameraControllerData _cameraControllerData;

        private readonly Transform _cameraTransform;

        private const float DefaultMaxVerticalAngle = 90f;
        private const float DefaultMaxHorizontalAngle = 180f;

        public FPSCameraController(FPSCameraSerializableComponents serializableComponents,
            IInputProvider inputProvider,
            CameraControllerData cameraControllerData, Transform moveCameraTransform)
        {
            _serializableComponents = serializableComponents;
            _inputProvider = inputProvider;
            _cameraControllerData = cameraControllerData;
            _cameraTransform = moveCameraTransform;

            ReturnDefaultConstraints();
        }

        public void SetIsEnabledRotateCameraByLookInput(bool isEnabled)
        {
            _isEnabledRotateCameraByLookInput = isEnabled;

            _serializableComponents.MonoBehaviourObserver.LateUpdated -= TryRotateCameraByLookInput;

            if (isEnabled)
            {
                _serializableComponents.MonoBehaviourObserver.LateUpdated += TryRotateCameraByLookInput;
            }
        }

        public void SetShouldRotateByLookInputX(bool should)
        {
            _shouldRotateByLookInputX = should;
        }

        public void TryRotateCamera(float offsetY, float offsetX)
        {
            if (offsetY == 0 && offsetX == 0)
                return;

            _xRotation -= offsetY;
            
            _xRotation = Mathf.Clamp(_xRotation, _minVerticalAngle, _maxVerticalAngle);

            _yRotation += offsetX;

            if (_minHorizontalAngle != 0 || _maxHorizontalAngle != 0)
                _yRotation = Mathf.Clamp(_yRotation, _minHorizontalAngle, _maxHorizontalAngle);
            
            var rotation = _cameraTransform.rotation;
            rotation = Quaternion.Euler(_xRotation, _shouldRotateByLookInputX ? _yRotation : rotation.eulerAngles.y,
                rotation.eulerAngles.z);

            _cameraTransform.rotation = rotation;
        }

        public void SetPosition(Vector3 position)
        {
            _cameraTransform.position = position;
        }

        public void SetRotation(Vector3 eulerAngles, bool clampRotation = true)
        {
            _cameraTransform.rotation = Quaternion.Euler(eulerAngles);
            _xRotation = clampRotation ? Mathf.DeltaAngle(0, eulerAngles.x) : eulerAngles.x;
            _yRotation = clampRotation ? Mathf.DeltaAngle(0, eulerAngles.y) : eulerAngles.y; 
        }

        public void SetVerticalAngleConstraints(float minAngles, float maxAngles)
        {
            _minVerticalAngle = minAngles;
            _maxVerticalAngle = maxAngles;
        }

        public void SetHorizontalAngleConstraints(float minAngles, float maxAngles)
        {
            _minHorizontalAngle = minAngles;
            _maxHorizontalAngle = maxAngles;
        }

        public Vector3 GetSmoothedLookVector()
        {
            return _inputProvider.LookAction *
                   (_cameraControllerData.GetXRotationSpeedFunc());
        }

        private void TryRotateCameraByLookInput()
        {
            if (CursorSwitchTools.IsCursorEnabled || !_isEnabledRotateCameraByLookInput) return;

            var lookVector = GetSmoothedLookVector() * Time.deltaTime;

            TryRotateCamera(lookVector.y, lookVector.x);
        }

        public void ReturnDefaultConstraints()
        {
            SetVerticalAngleConstraints(-DefaultMaxVerticalAngle, DefaultMaxVerticalAngle);
            SetHorizontalAngleConstraints(-DefaultMaxHorizontalAngle, DefaultMaxHorizontalAngle);
        }
    }
}