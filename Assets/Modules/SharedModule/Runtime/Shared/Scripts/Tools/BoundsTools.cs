using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class BoundsTools
    {
        public static Vector3 BottomCenter(this Bounds bounds, float offset = 0f)
        {
            return new Vector3(
                bounds.center.x,
                bounds.min.y + offset,
                bounds.center.z
            );
        }
    }
}