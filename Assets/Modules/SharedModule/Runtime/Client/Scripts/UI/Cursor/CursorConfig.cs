using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Client.Scripts.UI.Cursor
{
    [Serializable]
    public class CursorConfig
    {
        [field: Tooltip("Точка, где находится клик-часть курсора. 0,0 - это левая нижняя часть. Чтобы получить центр изображения, выберите x,x - где x это размер текстуры поделить над два")]
        [field: SerializeField] public Vector2 HotSpot { get; private set; }
        [field: SerializeField] public Texture2D Texture { get; private set; }
    }
}