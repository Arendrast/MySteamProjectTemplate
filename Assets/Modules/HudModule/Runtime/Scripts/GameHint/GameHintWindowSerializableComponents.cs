using TMPro;
using UnityEngine;

namespace Modules.HudModule.Runtime.Scripts.GameHint
{
    public class GameHintWindowSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI HintText { get; private set; }
    }
}