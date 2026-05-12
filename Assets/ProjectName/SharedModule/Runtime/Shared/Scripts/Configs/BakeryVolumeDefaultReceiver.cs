#if BAKERY
using UnityEngine;

// Example script, a counterpart to BakeryVolumeTrigger.cs (see that script for more info)

namespace CrazySWAT.SharedModule.Runtime.Shared.Scripts.Configs
{
    public class BakeryVolumeDefaultReceiver : MonoBehaviour
    {
        public bool forceUsage = false;

        // used by triggers
        internal int enterCounter = 0;
        internal BakeryVolumeTrigger movableTrigger = null;

        Renderer[] renderers;
        MaterialPropertyBlock current;

        // Cache renderers affected by volumes
        void Awake()
        {
            if (renderers == null) renderers = GetComponentsInChildren<Renderer>() as Renderer[];
            if (forceUsage)
            {
                //  HDRP can sometimes (?) fail to use globally set volumes when SRP batching is enabled, so disable it for this object.
                SetPropertyBlock(new MaterialPropertyBlock());
            }
        }

        // Called by triggers
        public void SetPropertyBlock(MaterialPropertyBlock mb)
        {
            if (renderers == null) renderers = GetComponentsInChildren<Renderer>() as Renderer[];
            for(int i=0; i<renderers.Length; i++)
            {
                renderers[i].SetPropertyBlock(mb);
            }
            current = mb;
        }

        // Update shader properties here if the volume is moving
        void LateUpdate()
        {
            if (movableTrigger == null) return;

            movableTrigger.UpdateBounds();
            SetPropertyBlock(current);
        }
    }
}
#endif