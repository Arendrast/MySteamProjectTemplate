using ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;

namespace ProjectName.EntityModule.Runtime.Shared.Scripts.Effects.Network
{
    public readonly struct EffectActionData
    {
        public readonly EffectType EffectType;
        public readonly EffectActionType EffectActionType;
        public readonly EffectOrigin EffectOrigin;
        public readonly int EffectableNetworkObjectId;
        public readonly int ApplierId;
        public readonly float TimeBeforeCancel;

        public EffectActionData(EffectType effectType, int effectableNetworkObjectId, int applierId,
            EffectActionType effectActionType, EffectOrigin effectOrigin, float timeBeforeCancel)
        {
            EffectType = effectType;
            EffectableNetworkObjectId = effectableNetworkObjectId;
            EffectActionType = effectActionType;
            EffectOrigin = effectOrigin;
            ApplierId = applierId;
            TimeBeforeCancel = timeBeforeCancel;
        }
    }
}