using UnityEngine;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
{
    public interface IPlayerSpawnerPositionsProvider
    {
        Transform[] SpawnersPositions { get; }
    }
}