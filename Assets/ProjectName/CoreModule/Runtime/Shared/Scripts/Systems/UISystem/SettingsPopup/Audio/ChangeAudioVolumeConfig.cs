using System;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Systems.UISystem.SettingsPopup.Audio
{
    [Serializable]
    public class ChangeAudioVolumeConfig
    {
        [field: SerializeField] public AudioType AudioType { get; private set; }
        [field: Range(10, 100)] [field: SerializeField] public float VolumeMultiplier { get; private set; } = 20;
    }
}