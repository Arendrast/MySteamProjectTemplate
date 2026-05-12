using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.HudModule.Runtime.Scripts.Positioners
{
    public class EllipsePartsPositioner : MonoBehaviour
    {
        public RectTransform[] elements;

        [Header("Settings")]
        public float startAngle = 0f;
        public float margin = 50f; // Отступ от краев экрана
    
        [Tooltip("Если true, радиусы будут вычислены автоматически по размеру экрана")]
        public bool fitToScreen = true;

        [Header("Manual Radius (if fitToScreen is false)")]
        public float radiusX = 500f;
        public float radiusY = 300f;

        [Button("Build Ellipse")] // Аналог [Button] для стандартного инспектора
        public void BuildEllipse()
        {
            if (elements == null || elements.Length == 0) return;

            // Определяем радиусы
            float currentRadiusX = radiusX;
            float currentRadiusY = radiusY;

            if (fitToScreen)
            {
                // Берем размер родителя (обычно это Canvas или полноэкранная панель)
                RectTransform parentRect = transform.parent as RectTransform;
                if (parentRect != null)
                {
                    // Радиус — это половина ширины/высоты минус отступ
                    currentRadiusX = (parentRect.rect.width / 2f) - margin;
                    currentRadiusY = (parentRect.rect.height / 2f) - margin;
                }
            }

            float angleStep = 360f / elements.Length;

            for (int i = 0; i < elements.Length; i++)
            {
                float angle = startAngle + angleStep * i;
                float rad = angle * Mathf.Deg2Rad;

                // Формула овала: x = cos * rX, y = sin * rY
                Vector2 pos = new Vector2(
                    Mathf.Cos(rad) * currentRadiusX,
                    Mathf.Sin(rad) * currentRadiusY
                );

                var rt = elements[i];

                // Центрируем элемент (убедись, что Anchor у элементов — Middle/Center)
                rt.anchoredPosition = pos;
            
                // Вращение (лицом к центру или от центра)
                // Если нужно, чтобы элементы "смотрели" наружу овала:
                rt.localRotation = Quaternion.Euler(0, 0, angle - 90f);
            }
        }
    }
}