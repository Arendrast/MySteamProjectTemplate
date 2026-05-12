using System;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerPredicate;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers.Overlap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts.LevelZoneEnterStateTrigger
{
    [Serializable]
    public class LevelZoneEnterStateTriggerConfig : IActionTriggerConfig
    {
        public bool IsInvalidData => !ShouldCheckAllPlayersInZone && RequiredPlayersInZoneNumber <= 0 || ZoneBoxOverlapObserver == null;
        [field: SerializeField] public bool ShouldCheckAllPlayersInZone { get; private set; }
        [field: SerializeField] public bool CheckForAliveState { get; private set; }

        [field: HideIf(nameof(ShouldCheckAllPlayersInZone))]
        [field: SerializeField]
        public int RequiredPlayersInZoneNumber { get; private set; }

        [field: SerializeField] public BoxOverlapObserver ZoneBoxOverlapObserver { get; private set; }
    }
}