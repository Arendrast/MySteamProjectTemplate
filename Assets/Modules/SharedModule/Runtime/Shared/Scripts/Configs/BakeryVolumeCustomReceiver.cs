#if BAKERY
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Configs
{
    public class BakeryVolumeCustomReceiver : MonoBehaviour
    {
        [Tooltip("If null, will auto-find the global BakeryVolume")]
        public BakeryVolume Volume;

        private MaterialPropertyBlock _block;
        private Renderer _renderer;

        private static int _minimalVolume, _volumeInvSize;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _block = new MaterialPropertyBlock();

            if (Volume == null)
                Volume = BakeryVolume.globalVolume;

            if (Volume == null)
                Volume = FindAnyObjectByType<BakeryVolume>();

            Apply();
        }

        private void Apply()
        {
            if (Volume == null || _renderer == null) return;
            if (Volume.bakedTexture0 == null) return;

            // Update bounds to current world position (prefab may have moved since bake)
            Volume.UpdateBounds();

            _renderer.GetPropertyBlock(_block);

            _block.SetTexture("_Volume0", Volume.bakedTexture0);
            _block.SetTexture("_Volume1", Volume.bakedTexture1);
            _block.SetTexture("_Volume2", Volume.bakedTexture2);
            if (Volume.bakedTexture3 != null) _block.SetTexture("_Volume3", Volume.bakedTexture3);
            if (Volume.bakedMask != null) _block.SetTexture("_VolumeMask", Volume.bakedMask);

            if (_minimalVolume == 0) _minimalVolume = Shader.PropertyToID("_VolumeMin");
            if (_volumeInvSize == 0) _volumeInvSize = Shader.PropertyToID("_VolumeInvSize");

            _block.SetVector(_minimalVolume, Volume.GetMin());
            _block.SetVector(_volumeInvSize, Volume.GetInvSize());

            if (Volume.supportRotationAfterBake) _block.SetMatrix("_VolumeMatrix", Volume.GetMatrix());
            if (Volume.rotateAroundY) _block.SetVector("_VolumeRY", Volume.GetRotationY());

            _block.SetVector("_VolumeVoxelSize", new Vector3(
                1.0f / Volume.bakedTexture0.width,
                1.0f / Volume.bakedTexture0.height,
                1.0f / Volume.bakedTexture0.depth));

            _renderer.SetPropertyBlock(_block);
        }
    }
}
#endif