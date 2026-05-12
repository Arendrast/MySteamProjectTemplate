using Modules.SharedModule.Runtime.Client.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.PausePopup
{
    public class PausePopupSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public Popup Popup { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }
        [field: SerializeField] public Button ExitButton { get; private set; }
        [field: SerializeField] public Button SecretButton { get; private set; }
        [field: SerializeField] public Button ExitToMenuButton { get; private set; }
    }
}