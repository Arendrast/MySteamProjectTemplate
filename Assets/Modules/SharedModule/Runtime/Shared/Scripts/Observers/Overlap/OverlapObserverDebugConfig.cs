using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Observers.Overlap
{
    [Serializable]
    public class OverlapObserverDebugConfig
    {
        [Header("Gizmos")] 
        [field: SerializeField] public Color SelectedColor { get; private set; } = Color.yellow;
        [field: SerializeField] public Color DefaultColor { get; private set; } = Color.green;
    }
}