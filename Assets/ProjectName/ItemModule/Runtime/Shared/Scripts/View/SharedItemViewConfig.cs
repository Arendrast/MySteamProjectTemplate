using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectName.ItemModule.Runtime.Shared.Scripts.View
{
    [Serializable]
    public class SharedItemViewConfig
    {
        [field: SerializeField] public Sprite SelectedItemSprite { get; private set; }
        [field: SerializeField] public Sprite DeselectedItemSprite { get; private set; }
        [field: SerializeField] public AssetReference PrefabReference { get; private set; }
    }
}