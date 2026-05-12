using System;
using UnityEngine;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Animations
{
    [Serializable]
    public class AnimancerStateSendPostEventConfig : IAnimancerStateEventConfig
    {
        [field: SerializeField] public int FrameNumber { get; private set; }
        [field: SerializeField] public string EventName { get; private set; }
    }
}