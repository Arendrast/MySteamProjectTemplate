using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.UI
{
    public class BarSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public Slider Slider { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Points { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MaxPoints { get; private set; }
    }
}