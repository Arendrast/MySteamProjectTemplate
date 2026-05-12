using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View
{
    public class PlayerSoundOriginsProviderSerializableComponents : MonoBehaviour
    {
        public enum PlayerSoundOriginType
        {
            Main,
            Steps
        }

        [Serializable]
        public class NetworkSendersProviderBySoundOriginType
        {
            [field: SerializeField] public PlayerSoundOriginType PlayerSoundOriginType { get; private set; }
        }

        public IReadOnlyDictionary<PlayerSoundOriginType, NetworkSendersProviderBySoundOriginType> NetworkSendersProviderBySoundOriginTypes
            => _networkSendersProviderBySoundOriginTypes.ToDictionary(pair => pair.PlayerSoundOriginType, pair => pair);

        [SerializeField] private List<NetworkSendersProviderBySoundOriginType> _networkSendersProviderBySoundOriginTypes;
    }
}