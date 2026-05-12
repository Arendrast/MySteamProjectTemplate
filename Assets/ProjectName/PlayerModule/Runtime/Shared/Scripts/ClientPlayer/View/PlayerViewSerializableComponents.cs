using System;
using System.Collections.Generic;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View.Rig;
using ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View
{
    public class PlayerViewSerializableComponents: MonoBehaviour
    {
        [Serializable]
        public class AssetReferenceByCharacterType
        {
            [field: SerializeField] public CharacterType CharacterType { get; private set; }
            [field: SerializeField] public AssetReference RigReference { get; private set; }
        }
        
        [field: SerializeField] public PlayerSoundOriginsProviderSerializableComponents SoundOriginsProviderSerializableComponents { get; private set; }
        [field: SerializeField] public AlternativeToCameraLooker CameraLooker { get; private set; }
        [field: SerializeField] public BarSerializableComponents HealthBar { get; private set; }
        [field: SerializeField] public List<AssetReferenceByCharacterType> RigsReferences { get; private set; }
        [field: SerializeField] public PlayerViewRigSerializableComponents ViewRigSerializableComponents { get; private set; }
    }
}