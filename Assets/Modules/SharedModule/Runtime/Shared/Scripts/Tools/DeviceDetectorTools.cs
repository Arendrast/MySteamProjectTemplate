using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class DeviceDetectorTools
    {
        public static bool IsMobile() => Application.isMobilePlatform;
    }
}