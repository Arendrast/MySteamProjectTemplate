using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class DeviceDetectorTools
    {
        public static bool IsMobile() => Application.isMobilePlatform;
    }
}