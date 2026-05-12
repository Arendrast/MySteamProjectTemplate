using System.Collections.Generic;
using FishNet.Managing.Client;
using ProjectName.ActionTriggerModule.Runtime.Shared.Scripts.TriggerReaction.Reactions.UpdateCounters;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.ClientPlayer;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.Movement;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.Reactors;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.FeetStates;
using ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.States.HandsStates;
using ProjectName.SharedModule.Runtime.Shared.Scripts.FiniteStateMachine;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Input;
using UnityEngine;
using VContainer;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.OwnerPlayer.OwnerStateMachine.StateMachinePart
{
    public class OwnerPlayerStateMachineInitializer : IOwnerPlayerComponent
    {
        private PlayerStateMachineBuilder<IFeetOwnerPlayerState> _feetStateMachineBuilder;
        private PlayerStateMachineBuilder<IHandsOwnerPlayerState> _handsStateMachineBuilder;

        private readonly IFeetOwnerPlayerState[] _statesForDirectlyEnterFromAimingOrUsing;

        private readonly IObjectResolver _resolver;
        private readonly IEnumerable<IOwnerPlayerStateReactor> _reactorsSubscribers;
        private readonly FiniteStateMachineModel<IFeetOwnerPlayerState> _feetStateMachineModel;
        private readonly FiniteStateMachineModel<IHandsOwnerPlayerState> _handsStateMachineModel;
        private readonly NetworkCountersSynchronizerBehaviourRepository _networkCountersSynchronizerBehaviourRepository;
        private readonly IPlayerSpawnerPositionsProvider _playerSpawnerPositionsProvider;
        private readonly ClientManager _clientManager;
        private readonly PlayerMovementStateController _playerMovementStateController;

        public OwnerPlayerStateMachineInitializer(IObjectResolver resolver,
            IEnumerable<IOwnerPlayerStateReactor> reactorsSubscribers,
            FiniteStateMachineModel<IFeetOwnerPlayerState> feetStateMachineModel,
            FiniteStateMachineModel<IHandsOwnerPlayerState> handsStateMachineModel,
            NetworkCountersSynchronizerBehaviourRepository networkCountersSynchronizerBehaviourRepository,
            IPlayerSpawnerPositionsProvider playerSpawnerPositionsProvider, ClientManager clientManager,
            PlayerMovementStateController playerMovementStateController)
        {
            _resolver = resolver;
            _reactorsSubscribers = reactorsSubscribers;
            _feetStateMachineModel = feetStateMachineModel;
            _handsStateMachineModel = handsStateMachineModel;
            _networkCountersSynchronizerBehaviourRepository = networkCountersSynchronizerBehaviourRepository;
            _playerSpawnerPositionsProvider = playerSpawnerPositionsProvider;
            _clientManager = clientManager;
            _playerMovementStateController = playerMovementStateController;

            _statesForDirectlyEnterFromAimingOrUsing = new IFeetOwnerPlayerState[]
            {
            };
        }

        public void Initialize(PlayerStateMachineBuilder<IFeetOwnerPlayerState> feetStateMachineBuilder,
            PlayerStateMachineBuilder<IHandsOwnerPlayerState> handsStateMachineBuilder, Transform cameraMoveTransform)
        {
            _feetStateMachineBuilder = feetStateMachineBuilder;
            _handsStateMachineBuilder = handsStateMachineBuilder;

            RegisterTransitionsForFeet();
            SubscribeToMediator();
            SubscribeToDirectlyEnterStates(cameraMoveTransform);

            feetStateMachineBuilder.Build();

            _feetStateMachineBuilder = null;
            _handsStateMachineBuilder = null;
        }

        private void RegisterTransitionsForFeet()
        {
            RegisterTransitionsToBaseMovementStates();
        }

        private void RegisterTransitionsToBaseMovementStates()
        {
            var inputProvider = _resolver.Resolve<IInputProvider>();

            _feetStateMachineBuilder
                .From<GroundIdleState>()
                .To<AirState>()
                .WhenAndAndGoBackWhenReverse(InAir);

            _feetStateMachineBuilder
                .From<GroundIdleState>()
                .To<GroundMovementState>()
                .WhenAndAndGoBackWhenReverse(() => inputProvider.MoveAction != Vector2.zero && !InAir());
        }

        private void SubscribeToDirectlyEnterStates(Transform cameraMoveTransform)
        {
        }

        private bool InAir()
        {
            return _playerMovementStateController.InAir();
        }

        private void SubscribeToMediator()
        {
            var mediator = new OwnerPlayerStateMachineMediator(_reactorsSubscribers);

            _feetStateMachineModel.EnteredState += mediator.SubscribeOnEnterState;
            _feetStateMachineModel.ExitedState += mediator.SubscribeOnExitState;
            _feetStateMachineModel.ChangedState += mediator.SubscribeOnChangeState;
            _feetStateMachineModel.UpdatedState += mediator.SubscribeOnUpdate;
            
            _handsStateMachineModel.EnteredState += mediator.SubscribeOnEnterState;
            _handsStateMachineModel.ExitedState += mediator.SubscribeOnExitState;
            _handsStateMachineModel.ChangedState += mediator.SubscribeOnChangeState;
            _handsStateMachineModel.UpdatedState += mediator.SubscribeOnUpdate;
        }
    }
}