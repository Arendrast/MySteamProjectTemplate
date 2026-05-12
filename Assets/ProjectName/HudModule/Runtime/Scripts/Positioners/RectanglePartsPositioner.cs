using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.HudModule.Runtime.Scripts.Positioners
{
    public class CustomRectanglePositioner : MonoBehaviour
    {
        public RectTransform[] elements;

        [Header("Side Counts (Distribution)")]
        public int topCount = 10;
        public int bottomCount = 10;
        public int leftCount = 3;
        public int rightCount = 3;

        [Header("Rectangle Dimensions")]
        [Tooltip("Ширина области, по которой будут расставлены элементы")]
        public float width = 800f;
        [Tooltip("Высота области, по которой будут расставлены элементы")]
        public float height = 500f;

        public float marginX;
        public float marginY;

        [Header("Visual Settings")]
        public bool rotateToCenter = true;
        public float rotationOffset = -90f; // Подстройка угла, если стрелки смотрят не туда

        [Button("Build Rectangle")]
        public void BuildRectangle()
        {
            if (elements == null || elements.Length == 0) return;

            int totalNeeded = topCount + bottomCount + leftCount + rightCount;
            if (elements.Length != totalNeeded)
            {
                Debug.LogWarning($"Массив элементов ({elements.Length}) не совпадает с суммой сторон ({totalNeeded})!");
            }

            // Половина размеров для расчетов от центра
            float halfW = width / 2f;
            float halfH = height / 2f;

            int currentIndex = 0;

            // 1. ВЕРХ (Top) - слева направо
            PositionLine(ref currentIndex, topCount, new Vector2(-halfW, halfH), new Vector2(halfW, halfH), 0f, true, new Vector2(0.5f, 1));

            // 2. ПРАВО (Right) - сверху вниз
            PositionLine(ref currentIndex, rightCount, new Vector2(halfW, halfH), new Vector2(halfW, -halfH), -90f, false, new Vector2(1, 0.5f));

            // 3. НИЗ (Bottom) - справа налево
            PositionLine(ref currentIndex, bottomCount, new Vector2(halfW, -halfH), new Vector2(-halfW, -halfH), 180f, true, new Vector2(0.5f, 0));

            // 4. ЛЕВО (Left) - снизу вверх
            PositionLine(ref currentIndex, leftCount, new Vector2(-halfW, -halfH), new Vector2(-halfW, halfH), 90f, false, new Vector2(0, 0.5f));
        }

        private void PositionLine(ref int startIndex, int count, Vector2 startPos, Vector2 endPos, float baseAngle, bool isVertical, Vector2 anchors)
        {
            if (count <= 0) return;

            for (var i = 0; i < count; i++)
            {
                if (startIndex >= elements.Length) break;
                
                var t = (float)(i + 1) / (count + 1);
                var pos = Vector2.Lerp(startPos, endPos, t);

                if (isVertical)
                    pos.y = -marginY * Mathf.Sign(pos.y);
                else
                    pos.x = -marginX * Mathf.Sign(pos.x);
            
                var rt = elements[startIndex];

                rt.anchorMin = anchors;
                rt.anchorMax = anchors;
                
                rt.anchoredPosition = pos;
                
                if (rotateToCenter)
                {
                    rt.localRotation = Quaternion.Euler(0, 0, baseAngle + rotationOffset);
                }

                startIndex++;
            }
        }
    }
}