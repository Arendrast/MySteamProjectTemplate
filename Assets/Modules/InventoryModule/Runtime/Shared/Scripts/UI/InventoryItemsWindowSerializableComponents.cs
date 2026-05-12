using UnityEngine;
using UnityEngine.UI;

namespace Modules.InventoryModule.Runtime.Shared.Scripts.UI
{
    public class InventoryItemsWindowSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public float TimeBeforeDoFade { get; private set; } = 3;
        [field: SerializeField] public float FadeTime { get; private set; } = 1;
        [field: SerializeField] public float FadedBackgroundAlphaValue { get; private set; } = 0.4f;
        [field: SerializeField] public float FadedItemsImagesAlphaValue { get; private set; } = 0.5f;
        
        [field: SerializeField] public Image BackgroundImage { get; private set; }
        
        [field: SerializeField]
        public InventoryItemBlockSerializableComponents[] InventoryItemsBlockSerializableComponents
        {
            get;
            private set;
        }
    }
}