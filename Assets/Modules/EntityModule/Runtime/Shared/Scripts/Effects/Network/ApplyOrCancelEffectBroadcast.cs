using FishNet.Broadcast;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.Logic;

namespace Modules.EntityModule.Runtime.Shared.Scripts.Effects.Network
{
    public readonly struct ApplyOrCancelEffectBroadcast : IBroadcast
    {
        public readonly EffectType EffectType;
        public readonly EffectOrigin EffectOrigin;
        public readonly EffectActionType EffectActionType;
        public readonly int ApplierId, ReceiverId;
        public readonly float TimeBeforeCancel;


        public ApplyOrCancelEffectBroadcast(EffectType effectType, int receiverId, int applierId,
            EffectActionType effectActionType, float timeBeforeCancel, EffectOrigin effectOrigin)
        {
            EffectType = effectType;
            ReceiverId = receiverId;
            ApplierId = applierId;
            EffectActionType = effectActionType;
            TimeBeforeCancel = timeBeforeCancel;
            EffectOrigin = effectOrigin;
        }
    }
}