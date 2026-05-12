using Animancer;
using Modules.EntityModule.Runtime.Shared.Scripts.Effects.Effectable.View;
using Modules.NetworkModule.Runtime.Shared.Scripts.Animations;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers;
using Modules.NetworkModule.Runtime.Shared.Scripts.Synchronizers.SynchronizerBehaviours;
using Modules.SharedModule.Runtime.Client.Scripts.AnimationEventSystem;
using Modules.SharedModule.Runtime.Shared.Scripts.Animations;
using UnityEngine;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer.View.Rig
{
    public class PlayerViewRigSerializableComponents : MonoBehaviour
    {
        [field: SerializeField]
        public EffectableViewSerializableComponents EffectableViewSerializableComponents { get; private set; }

        [field: SerializeField] public Transform ItemsViewsPositionOrigin { get; private set; }

        [field: SerializeField]
        public NetworkVector3SynchronizerBehaviour LookVectorSynchronizerBehaviour { get; private set; }

        [field: SerializeField]
        public NetworkVector2SynchronizerBehaviour VelocitySynchronizerBehaviour { get; private set; }

        [field: SerializeField]
        public NetworkFloatSynchronizerBehaviour IKWeightSynchronizerBehaviour { get; private set; }

        [field: SerializeField] public AnimancerComponent AnimancerComponent { get; private set; }
        [field: SerializeField] public Transform PelvisTransform { get; private set; }
        [field: SerializeField] public AvatarMask LegsMask { get; private set; }
        [field: SerializeField] public AvatarMask UpperBodyMask { get; private set; }
        [field: SerializeField] public AnimationPlayer UpperBodyAnimationPlayer { get; private set; }
        [field: SerializeField] public NetworkAnimationPlayer LegsNetworkAnimationPlayer { get; private set; }
        [field: SerializeField] public NetworkAnimationPlayer UpperBodyNetworkAnimationPlayer { get; private set; }
        [field: SerializeField] public AnimationEventsObserver AnimationEventsObserver { get; private set; }
        [field: SerializeField] public Transform SkeletonRoot { get; private set; }
        [field: SerializeField] public Transform Rig { get; private set; }
    }
}