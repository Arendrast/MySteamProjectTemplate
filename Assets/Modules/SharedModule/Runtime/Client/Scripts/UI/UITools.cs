using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Modules.SharedModule.Runtime.Client.Scripts.UI
{
    public static class UITools
    {
        public static bool TrySimulateRealClick(Button button)
        {
            if (!button.gameObject.activeInHierarchy || !button.enabled || !button.interactable)
                return false;
            
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, button.transform.position);
            
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPoint
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // 4. Проверяем, является ли наша кнопка самым первым (верхним) объектом
            if (results.Count > 0 && results[0].gameObject == button.gameObject)
            {
                // Если да — вызываем событие нажатия
                ExecuteEvents.Execute(button.gameObject, eventData, ExecuteEvents.pointerClickHandler);
                return true;
            }
            
            return false;
        }
        
        public static List<RaycastResult> GetPointerEventRaycastResults()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results;
        }
    }
}