using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer
{
    public interface IPlayerSpawnerPositionsProvider
    {
        Transform[] SpawnersPositions { get; }
    }
}