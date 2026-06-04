using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using MoreLinq;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class CameraComponents
    {
        public readonly TwoDCameraMovementController twoDCameraMovementController;
        public readonly CameraSerializableComponents SerializableComponents;

        public CameraComponents(TwoDCameraMovementController twoDCameraMovementController,
            CameraSerializableComponents serializableComponents, FollowPositionController followPositionController,
            FollowRotationController followRotationController)
        {
            this.twoDCameraMovementController = twoDCameraMovementController;
            SerializableComponents = serializableComponents;
        }

        public void Dispose()
        {
            SerializableComponents.CameraParentsTransformsByType.Values.ForEach(transform =>
                transform.localPosition = Vector3.zero);
            SerializableComponents.Camera.transform.localPosition = Vector3.zero;
            twoDCameraMovementController.SetPosition(Vector3.zero);
        }
    }
}