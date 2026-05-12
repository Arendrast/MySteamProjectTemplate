using ProjectName.SharedModule.Runtime.Shared.Scripts.Pulsation;
using UnityEngine;

namespace ProjectName.HudModule.Runtime.Scripts.LowHealPoints
{
    public class LowHealPointsPopupSerializableComponents : MonoBehaviour
    {
        [field: SerializeField] public int StartHealthPointsPercentageForShow { get; private set; }

        [field: SerializeField] public AnimationCurve VignetteValueAnimationCurve { get; private set; }
        [field: SerializeField] public float VignetteStartValue { get; private set; }
        [field: SerializeField] public float VignetteEndValue { get; private set; }
        [field: SerializeField] public PulsationConfig VignettePulsationConfig { get; private set; }
        [field: SerializeField] public float SetVignetteValueTimeOnSetHealthPoints { get; private set; } = 0.1f;
    }
}