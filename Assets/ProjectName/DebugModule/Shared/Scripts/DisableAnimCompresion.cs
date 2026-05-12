#if UNITY_EDITOR
using UnityEditor;

namespace ProjectName.DebugModule.Shared.Scripts
{
    public class DisableAnimCompression : AssetPostprocessor
    {
        void OnPreprocessAnimation()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            if (modelImporter != null)
            {
                modelImporter.animationCompression = ModelImporterAnimationCompression.Off;
            }
        }
    }
}
#endif