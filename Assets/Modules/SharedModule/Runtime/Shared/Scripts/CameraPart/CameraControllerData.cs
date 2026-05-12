using System;

namespace Modules.SharedModule.Runtime.Shared.Scripts.CameraPart
{
    public class CameraControllerData
    {
        public readonly Func<float> GetXRotationSpeedFunc;
        
        public CameraControllerData(Func<float> getXRotationSpeedFunc)
        {
            GetXRotationSpeedFunc = getXRotationSpeedFunc;
        }
    }
}