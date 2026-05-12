using TMPro;
using UnityEngine;

namespace ProjectName.HudModule.Runtime.Scripts.GameHint
{
    public class GameHintWindowSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI HintText { get; private set; }
    }
}