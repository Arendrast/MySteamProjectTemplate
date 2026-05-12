using UnityEngine.UI;

namespace ProjectName.SharedModule.Runtime.Client.Scripts.UI
{
    public class RaycastTarget : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }
    }
}