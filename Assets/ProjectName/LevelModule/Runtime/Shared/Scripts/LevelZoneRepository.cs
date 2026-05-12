using System;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using UnityEngine;

namespace ProjectName.LevelModule.Runtime.Shared.Scripts
{
    public class LevelZoneRepository : IMatchSharedService, IDisposable, IPlayerSpawnerPositionsProvider
    {
        public LevelZoneSerializableComponents TargetLevelZoneSerializableComponents { get; private set; }
        public Transform[] SpawnersPositions => TargetLevelZoneSerializableComponents.SpawnersPositions;


        public LevelZoneSerializableComponents PersistentObjectsLevelZoneSerializableComponents
        {
            get;
            private set;
        }

        public void Dispose()
        {
            TargetLevelZoneSerializableComponents = null;
            PersistentObjectsLevelZoneSerializableComponents = null;
        }

        public void SetTargetZoneEnvironmentSerializableComponents(
            LevelZoneSerializableComponents targetLevelZoneSerializableComponents)
        {
            TargetLevelZoneSerializableComponents = targetLevelZoneSerializableComponents;
        }

        public void SetPersistentObjectsZoneEnvironmentSerializableComponents(
            LevelZoneSerializableComponents targetLevelZoneSerializableComponents)
        {
            PersistentObjectsLevelZoneSerializableComponents = targetLevelZoneSerializableComponents;
        }
    }
}