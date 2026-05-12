using MoreLinq;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class CameraComponents
    {
        public readonly FPSCameraController FPSCameraController;
        public readonly CameraSerializableComponents SerializableComponents;
        public readonly FollowPositionController FollowPositionController;
        public readonly FollowRotationController FollowRotationController;

        public CameraComponents(FPSCameraController fpsCameraController,
            CameraSerializableComponents serializableComponents, FollowPositionController followPositionController,
            FollowRotationController followRotationController)
        {
            FPSCameraController = fpsCameraController;
            SerializableComponents = serializableComponents;
            FollowPositionController = followPositionController;
            FollowRotationController = followRotationController;
        }

        public void Dispose()
        {
            SerializableComponents.CameraParentsTransformsByType.Values.ForEach(transform =>
                transform.localPosition = Vector3.zero);
            SerializableComponents.Camera.transform.localPosition = Vector3.zero;
            FPSCameraController.ReturnDefaultConstraints();
            FPSCameraController.SetPosition(Vector3.zero);
            FPSCameraController.SetRotation(Vector3.zero);
            FPSCameraController.SetIsEnabledRotateCameraByLookInput(false);
            FPSCameraController.SetShouldRotateByLookInputX(false);
            FollowPositionController.EndFollow();
            FollowRotationController.EndFollow();
        }
    }
}