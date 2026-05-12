using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Damage;
using ProjectName.EntityModule.Runtime.Shared.Scripts.Push;
using ProjectName.InventoryModule.Runtime.Shared.Scripts;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using ProjectName.SharedModule.Runtime.Shared.Scripts.CameraPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Configs;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Observers;
using ProjectName.SharedModule.Runtime.Shared.Scripts.PhysicsPart;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.DependencyInjection
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
            
            public CharacterControllerPushHandlerController PushHandlerController { get; set; }
            public CharacterControllerPushHandlerModel PushHandlerModel { get; set; }
            
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
            var characterController = _dependencies.ClientPlayerComponents.SerializableComponents.CharacterController;

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
                _dependencies.CameraComponents.FPSCameraController,
                _dependencies.CameraComponents,
                _dependencies.ClientPlayerComponents.EntityComponents.DamageReceiverModel,
                _dependencies.ClientPlayerComponents.EntityComponents.DamageDealerModel,
                _dependencies.ClientPlayerComponents.NotOwnerInteractionVisitor,
                _dependencies.ClientPlayerComponents.ViewComponents,
                _dependencies.OwnerPlayerSerializableComponents.GetComponent<NetworkObject>(),
                _dependencies.ClientPlayerComponents.SerializableComponents.Observer,
                _dependencies.ClientPlayerComponents.SerializableComponents.Animator,
                _dependencies.ClientPlayerComponents.SerializableComponents.CharacterController,
                _dependencies.ClientPlayerComponents.SerializableComponents.ManyInvokableOneFrameCharacterController,
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
                .CharacterControllerCollider).As<Collider>();

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
                var radiusFunc =
                    new Func<float>(() => characterController.radius - 0.05f); // must less than CharController radius 

                return new IsGroundedProvider(
                    _dependencies.PhysicsLayersConfig,
                    () => characterController.bounds.center.WithY(characterController.bounds.min.y) +
                          characterController.transform.up * radiusFunc.Invoke(),
                    radiusFunc,
                    GetFirstRaycastHitExceptOwner);

                RaycastHit GetFirstRaycastHitExceptOwner(RaycastHit[] hits, int hitsCount)
                {
                    return PlayerTools.GetNearestRaycastHitExceptOwner(hits, hitsCount,
                        _dependencies.OwnerPlayerSerializableComponents.transform, characterController.bounds.center);
                }
            }
        }
    }
}