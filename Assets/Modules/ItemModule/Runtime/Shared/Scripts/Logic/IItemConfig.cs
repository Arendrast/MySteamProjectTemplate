using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.ItemModule.Runtime.Shared.Scripts.Logic
{
    public interface IItemConfig : IIndexable
    {
        ItemType ItemType { get; }
        AssetReference PrefabReference { get; }
        Vector3 StartLocalRotation { get; }
        Vector3 StartLocalPosition { get; }
        
        float ItemOnRemoveFromSlotTime { get; }
        float ItemOnAddToSlotTime { get; }
    }
}