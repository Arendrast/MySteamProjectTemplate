using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class CursorSwitchTools
    {
        public static bool IsCursorEnabled = true;

        public static void TrySwitchCursor(bool? isEnabled = null)
        {
            if (DeviceDetectorTools.IsMobile())
                return;
            
            IsCursorEnabled = isEnabled ?? !IsCursorEnabled;
            Cursor.visible = isEnabled ?? IsCursorEnabled;
            Cursor.lockState = isEnabled ?? IsCursorEnabled ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public static void TryDisableCursor()
        {
            TrySwitchCursor(false);
        }

        public static void TryEnableCursor()
        {
            TrySwitchCursor(true);
        }
    }
}