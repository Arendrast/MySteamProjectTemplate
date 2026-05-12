using System;
using FishNet.Object;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.AliveStateTrigger
{
    [Serializable]
    public class AliveStateTriggerConfig : IActionTriggerConfig
    {
        public bool IsInvalidData => TargetObject == null;
        [field: SerializeField] public NetworkObject TargetObject { get; private set; }
        [field: SerializeField] public bool ShouldBeAlive { get; private set; }
    }
}