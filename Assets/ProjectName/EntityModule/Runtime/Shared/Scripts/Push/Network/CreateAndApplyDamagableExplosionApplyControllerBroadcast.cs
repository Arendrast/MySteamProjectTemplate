using FishNet.Broadcast;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Push.Network
{
    public struct CreateAndApplyDamagableExplosionApplyControllerBroadcast : IBroadcast
    {
        public readonly ExplosionData ExplosionData;
        public readonly int DamageDealerObjectId;

        public CreateAndApplyDamagableExplosionApplyControllerBroadcast(ExplosionData explosionData, int damageDealerObjectId)
        {
            ExplosionData = explosionData;
            DamageDealerObjectId = damageDealerObjectId;
        }
    }
}