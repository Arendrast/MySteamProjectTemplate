using UnityEngine;
using UnityEngine.UI;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.MainMenuPopup
{
    public class SelectLevelButtonSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public int LevelNumber { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Image SelectedImage { get; private set; }
        [field: SerializeField] public Image UnselectedImage { get; private set; }
    }
}