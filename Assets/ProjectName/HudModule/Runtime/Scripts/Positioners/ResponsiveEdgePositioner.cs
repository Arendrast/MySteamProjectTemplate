using UnityEngine;

namespace ProjectName.HudModule.Runtime.Scripts.Positioners
{
    [ExecuteInEditMode] // Чтобы можно было видеть изменения в редакторе
    public class ResponsiveEdgePositioner : MonoBehaviour
    {
        public RectTransform[] elements;

        [Header("Side Counts")]
        public int topCount = 10;
        public int bottomCount = 10;
        public int leftCount = 3;
        public int rightCount = 3;

        [Header("Margins")]
        [Tooltip("Отступ от краев экрана в пикселях")]
        public float marginX = 100f;
        public float marginY = 100f;

        [Header("Rotation")]
        public float rotationOffset = -90f;

        private RectTransform parentRect;

        private void Awake()
        {
            parentRect = GetComponent<RectTransform>();
        }

        // Эта функция вызывается Unity автоматически, когда меняется размер экрана в редакторе или игре
        private void OnRectTransformDimensionsChange()
        {
            BuildLayout();
        }

        [ContextMenu("Build Layout")]
        public void BuildLayout()
        {
            if (elements == null || elements.Length == 0) return;
            if (parentRect == null) parentRect = GetComponent<RectTransform>();

            // Получаем текущую ширину и высоту родителя (экрана)
            float screenW = parentRect.rect.width;
            float screenH = parentRect.rect.height;

            // Вычисляем границы с учетом отступов
            float halfW = (screenW / 2f) - marginX;
            float halfH = (screenH / 2f) - marginY;

            int currentIndex = 0;

            // Расставляем стороны
            // 1. ВЕРХ
            PositionLine(ref currentIndex, topCount, new Vector2(-halfW, halfH), new Vector2(halfW, halfH), 0f);
            // 2. ПРАВО
            PositionLine(ref currentIndex, rightCount, new Vector2(halfW, halfH), new Vector2(halfW, -halfH), -90f);
            // 3. НИЗ
            PositionLine(ref currentIndex, bottomCount, new Vector2(halfW, -halfH), new Vector2(-halfW, -halfH), 180f);
            // 4. ЛЕВО
            PositionLine(ref currentIndex, leftCount, new Vector2(-halfW, -halfH), new Vector2(-halfW, halfH), 90f);
        }

        private void PositionLine(ref int startIndex, int count, Vector2 startPos, Vector2 endPos, float baseAngle)
        {
            if (count <= 0) return;

            for (int i = 0; i < count; i++)
            {
                if (startIndex >= elements.Length) break;

                // Используем (i + 1) / (count + 1), чтобы элементы не залезали в углы
                float t = (float)(i + 1) / (count + 1);
                Vector2 pos = Vector2.Lerp(startPos, endPos, t);
            
                var rt = elements[startIndex];
            
                // Настройка якорей: ставим их в центр, чтобы координаты считались от центра экрана
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = pos;

                rt.localRotation = Quaternion.Euler(0, 0, baseAngle + rotationOffset);

                startIndex++;
            }
        }
    
        // Чтобы сработало при запуске игры
        private void Start()
        {
            BuildLayout();
        }
    }
}