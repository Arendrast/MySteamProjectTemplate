using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.HudModule.Runtime.Scripts.Positioners
{
    public class CirclePartsPositioner : MonoBehaviour
    {
        public RectTransform[] elements;

        [Header("Circle Settings")]
        public float radius = 100f;
        public float startAngle = 0f;

        [Button]
        public void BuildCircle()
        {
            float angleStep = 360f / elements.Length;

            for (int i = 0; i < elements.Length; i++)
            {
                float angle = startAngle + angleStep * i;
                float rad = angle * Mathf.Deg2Rad;

                Vector2 pos = new Vector2(
                    Mathf.Cos(rad) * radius,
                    Mathf.Sin(rad) * radius
                );

                var rt = elements[i];

                rt.anchoredPosition = pos;
                
                rt.localRotation = Quaternion.Euler(0, 0, angle - 90f);
            }
        }
    }
}