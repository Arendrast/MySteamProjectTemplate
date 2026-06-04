using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using Modules.EntityModule.Runtime.Shared.Scripts.Damage;
using Modules.EntityModule.Runtime.Shared.Scripts.Push;
using Modules.InventoryModule.Runtime.Shared.Scripts;
using Modules.OverlapModule.Runtime.Scripts;
using Modules.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using Modules.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using Modules.SharedModule.Runtime.Shared.Scripts.CameraPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Modules.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Modules.PlayerModule.Runtime.Shared.Scripts.DependencyInjection
{
    public class OwnerPlayerLifetimeScope : LifetimeScope
    {
        public struct Dependencies
        {
            public InventoryItemsConfig InventoryItemsConfig { get; set; }
            public MovementConfig MovementConfig { get; set; }
            public PhysicsLayersConfig PhysicsLayersConfig { get; set; }

            public InventoryItemsModel InventoryItemsModel { get; set; }

            public CameraComponents CameraComponents { get; set; }
            public Transform PlayerTransform { get; set; }
            public OwnerPlayerSerializableComponents OwnerPlayerSerializableComponents { get; set; }
            public ClientPlayerComponents ClientPlayerComponents { get; set; }
            public DamageReceiversRepository DamageReceiversRepository { get; set; }

            public PushHandlerController PushHandlerController { get; set; }
            public PushHandlerModel PushHandlerModel { get; set; }
        }

        private Dependencies _dependencies;
        private LifetimeScope _parent;

        public void SetDependenciesAndPrepareToBuild(Dependencies dependencies)
        {
            _dependencies = dependencies;
            GetRuntimeParent();
        }

        protected override LifetimeScope GetRuntimeParent()
        {
            return _parent ??= (LifetimeScope)FindAnyObjectByType(parentReference.Type);
        }

        protected override async UniTask ConfigureAsync(IContainerBuilder builder)
        {
            var capsuleCollider = _dependencies.ClientPlayerComponents.SerializableComponents.CapsuleCollider;

            builder.RegisterInstances(
                _dependencies.InventoryItemsConfig,
                _dependencies.PhysicsLayersConfig,
                _dependencies.MovementConfig,
                _dependencies.PlayerTransform,
                _dependencies.OwnerPlayerSerializableComponents,
                _dependencies.ClientPlayerComponents,
                _dependencies.ClientPlayerComponents.EntityComponents,
                _dependencies.InventoryItemsModel,
                _dependencies.ClientPlayerComponents.SerializableComponents.destroyCancellationToken,
                _dependencies.CameraComponents.SerializableComponents.Camera,
                _dependencies.CameraComponents.twoDCameraMovementController,
                _dependencies.CameraComponents,
                _dependencies.ClientPlayerComponents.EntityComponents.DamageReceiverModel,
                _dependencies.ClientPlayerComponents.EntityComponents.DamageDealerModel,
                _dependencies.ClientPlayerComponents.NotOwnerInteractionVisitor,
                _dependencies.ClientPlayerComponents.ViewComponents,
                _dependencies.ClientPlayerComponents.SerializableComponents.gameObject,
                _dependencies.OwnerPlayerSerializableComponents.GetComponent<NetworkObject>(),
                _dependencies.ClientPlayerComponents.SerializableComponents.Animator,
                _dependencies.OwnerPlayerSerializableComponents.ClientSerializableComponents.MovementComponent,
                _dependencies.ClientPlayerComponents.StateMachine,
                _dependencies.ClientPlayerComponents.EntityComponents.HealthModel,
                _dependencies.ClientPlayerComponents.ViewComponents.SerializableComponents
                    .SoundOriginsProviderSerializableComponents,
                _dependencies.ClientPlayerComponents.ViewComponents.ViewRigSerializableComponents,
                _dependencies.PushHandlerController,
                _dependencies.PushHandlerModel,
                GetIsGroundedProvider(),
                GetDamageReceiversFinder());

            builder.RegisterInstance(_dependencies.ClientPlayerComponents.SerializableComponents
                .CapsuleCollider).As<Collider>();

            builder.RegisterInstance(_dependencies.ClientPlayerComponents.SerializableComponents.CapsuleOverlapObserver)
                .As<IOverlapObserver>();

            builder.RegisterMany(Lifetime.Singleton,
                GetStateMachineTypes(),
                (await ReflectionTools.GetAllInheritorsTypesAsync<IOwnerPlayerComponent>(false)).ToArray(),
                new[]
                {
                    typeof(OwnerPlayerComponents), typeof(CurvedSpeedMovementCalculator),
                    typeof(CurvedSpeedMovementController)
                });

            builder.RegisterEntryPoint<OwnerPlayerDependenciesCreator>();

            return;

            Type[] GetStateMachineTypes() => new[]
            {
                typeof(FiniteStateMachineModel<IHandsOwnerPlayerState>),
                typeof(FiniteStateMachineController<IHandsOwnerPlayerState>),
                typeof(FiniteStateMachineModel<IFeetOwnerPlayerState>),
                typeof(FiniteStateMachineController<IFeetOwnerPlayerState>),
            };

            DamageReceiversFinder GetDamageReceiversFinder()
            {
                return new DamageReceiversFinder(_dependencies.DamageReceiversRepository,
                    _dependencies.ClientPlayerComponents.EntityComponents.DamageDealerModel,
                    () => _dependencies.ClientPlayerComponents.SerializableComponents.transform.position);
            }


            IsGroundedProvider GetIsGroundedProvider()
            {
                Func<float> radiusFunc = null;

#if TWO_D
                radiusFunc = () => capsuleCollider.size.x - 0.05f;
#else
                radiusFunc = () => capsuleCollider.radius - 0.05f;
#endif

                return new IsGroundedProvider(
                    _dependencies.PhysicsLayersConfig,
                    () => capsuleCollider.bounds.center.WithY(capsuleCollider.bounds.min.y) +
                          capsuleCollider.transform.up * radiusFunc.Invoke(),
                    radiusFunc,
                    GetFirstRaycastHitExceptOwner);

                RaycastHit GetFirstRaycastHitExceptOwner(RaycastHit[] hits, int hitsCount)
                {
                    return PlayerTools.GetNearestRaycastHitExceptOwner(hits, hitsCount,
                        _dependencies.OwnerPlayerSerializableComponents.transform, capsuleCollider.bounds.center);
                }
            }
        }
    }
}