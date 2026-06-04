using System;
using UnityEngine;

namespace Modules.OverlapModule.Runtime.Scripts
{
    [Serializable]
    public class OverlapObserverDebugConfig
    {
        [field: Header("Gizmos")] 
        [field: SerializeField] public Color SelectedColor { get; private set; } = Color.yellow;
        [field: SerializeField] public Color DefaultColor { get; private set; } = Color.green;
    }
}