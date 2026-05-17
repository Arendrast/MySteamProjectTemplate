using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure.GameStateMachine;

namespace Modules.AppModule.Runtime.Server.Scripts.Infrastructure.GameStateMachinePart
{
    public class ServerGameStateMachine : IMatchServerService
    {
        public event Action<IGameState, NetworkConnection> EnteredInState;

        private readonly Dictionary<NetworkConnection, IGameState> _activeGameStateByClientNetworkConnection =
            new Dictionary<NetworkConnection, IGameState>();

        private readonly Dictionary<Type, IGameState>
            _statesByType = new Dictionary<Type, IGameState>();

        private readonly Dictionary<ServerGameStateType, IGameState> _statesByEnum =
            new Dictionary<ServerGameStateType, IGameState>();

        public ServerGameStateMachine(ClientInitializationGameState clientInitializationGameState,
            HostMatchGameState hostMatchGameState,
            NetworkManager networkManager)
        {
            RegisterState(clientInitializationGameState, ServerGameStateType.ClientInitialization);
            RegisterState(hostMatchGameState, ServerGameStateType.ClientMatch);
        }

        public void Exit(NetworkConnection networkConnection) => _activeGameStateByClientNetworkConnection
            .GetValueOrDefault(networkConnection)?.ExitAsync(networkConnection);


        private void RemoveNetworkConnectionFromActiveGameStates(NetworkConnection networkConnection) =>
            _activeGameStateByClientNetworkConnection.Remove(networkConnection);

        public void Enter(ServerGameStateType gameStateType, NetworkConnection networkConnection)
            => Enter(_statesByEnum[gameStateType], networkConnection);

        private void Enter<TState>(TState state, NetworkConnection networkConnection)
            where TState : class, IGameState
        {
            state.EnterAsync(networkConnection).Forget();

            _activeGameStateByClientNetworkConnection.GetValueOrDefault(networkConnection)
                ?.ExitAsync(networkConnection);

            EnteredInState?.Invoke(state, networkConnection);
            _activeGameStateByClientNetworkConnection.TryAdd(networkConnection, state);
        }

        private async void InitializeAsync(NetworkManager networkManager)
        {
            // start host connection
        }

        private TState GetState<TState>() where TState : class, IGameState =>
            _statesByType[typeof(TState)] as TState;

        private void RegisterState<TState>(TState implementation, ServerGameStateType? state = null)
            where TState : class, IGameState
        {
            _statesByType.Add(typeof(TState), implementation);

            if (state != null)
                _statesByEnum.Add((ServerGameStateType)state, implementation);
        }
    }
}