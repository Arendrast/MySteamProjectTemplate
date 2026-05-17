using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Client.Scripts.GameStateMachine;
using Modules.SharedModule.Runtime.Shared.Scripts.GameStateMachine;

namespace Modules.AppModule.Runtime.Shared.Scripts.GameStateMachinePart
{
    public class GameStateMachine
    {
        public IGameState ActiveGameState { get; private set; }

        private readonly Dictionary<GameStateType, IGameState> _statesByEnum =
            new Dictionary<GameStateType, IGameState>();

        private readonly Dictionary<Type, IGameState>
            _statesByType = new Dictionary<Type, IGameState>();

        public GameStateMachine(BootstrapGameState bootstrapGameState, 
            ClientMainMenuGameState clientMainMenuGameState, MatchGameState matchGameState, ExitGameState exitGameState)
        {
            RegisterState(bootstrapGameState);
            RegisterState(clientMainMenuGameState, GameStateType.MainMenu);
            RegisterState(matchGameState, GameStateType.MatchGame);
            RegisterState(exitGameState, GameStateType.Exit);
            
            InitializeAsync();
        }

        public async UniTask TryEnterStateAsync<TState>(IGameStateEnterData data)
            where TState : class, IGameState =>
            await TryEnterStateAsync(GetState<TState>(), data);
        
        private async void InitializeAsync()
        {
            await UniTask.Yield();
            await TryEnterStateAsync<BootstrapGameState>(null);
        }

        public async UniTask TryEnterStateAsync(EnterGameStateEvent @event) =>
            await TryEnterStateAsync(_statesByEnum[@event.GameStateType] as IGameState,
                @event.GameStateEnterData);

        private async UniTask TryChangeStateAsync<TState>(TState state)
            where TState : class, IGameState
        {
            if (state == null)
                return;

            if (ActiveGameState != null)
                await ActiveGameState.ExitAsync();

            ActiveGameState = state;
        }

        private async UniTask TryEnterStateAsync<TState>(TState state, IGameStateEnterData data = null)
            where TState : class, IGameState
        {
            if (state == null)
                return;

            await TryChangeStateAsync(state);
            await state.EnterAsync(data);
        }

        private TState GetState<TState>() where TState : class, IGameState =>
            _statesByType.GetValueOrDefault(typeof(TState)) as TState;

        private void RegisterState<TState>(TState implementation,
            GameStateType? state = null)
            where TState : class, IGameState
        {
            _statesByType.Add(typeof(TState), implementation);

            if (state != null)
                _statesByEnum.Add((GameStateType)state, implementation);
        }
    }
}