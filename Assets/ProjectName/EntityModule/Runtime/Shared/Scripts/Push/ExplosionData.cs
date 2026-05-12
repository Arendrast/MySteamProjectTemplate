using UnityEngine;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push
{
    public struct ExplosionData
    {
        public readonly Vector3 Position;
        public readonly ExplosionForceData ExplosionForceData;
        public readonly int Damage;
        public readonly LayerMask LayerMask;
        public readonly bool ShouldMoveByYAxis;
        public readonly GameObject[] ExcludedGameObjects;
        
        public ExplosionData(Vector3 position, ExplosionForceData explosionForceData, int damage,
            LayerMask layerMask, GameObject[] excludedGameObjects, bool shouldMoveByYAxis)
        {
            Position = position;
            ExplosionForceData = explosionForceData;
            Damage = damage;
            LayerMask = layerMask;
            ExcludedGameObjects = excludedGameObjects;
            ShouldMoveByYAxis = shouldMoveByYAxis;
        }

        public ExplosionData WithLayerMask(LayerMask layerMask)
        {
            return new ExplosionData(Position, ExplosionForceData, Damage, layerMask, ExcludedGameObjects, ShouldMoveByYAxis);
        }
        
        public ExplosionData WithExcludedGameObjects(GameObject[] excludedGameObjects)
        {
            return new ExplosionData(Position, ExplosionForceData, Damage, LayerMask, excludedGameObjects, ShouldMoveByYAxis);
        }
    }
}