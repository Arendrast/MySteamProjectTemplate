using Cysharp.Threading.Tasks;
using FishNet.Managing;
using FishNet.Managing.Transporting;
using FishNet.Transporting;
using Modules.CoreModule.Runtime.Shared.Scripts.GameStateMachinePart;
using Modules.SharedModule.Runtime.Client.Scripts.UI.Cursor;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using Modules.SharedModule.Runtime.Shared.Scripts.Input;
using Modules.SharedModule.Runtime.Shared.Scripts.Loading;
using Modules.SharedModule.Runtime.Shared.Scripts.Observers;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class PersistentServicesScope : LifetimeScope
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;
        [SerializeField] private Transport _unityTransport;
        [SerializeField] private MatchServicesScopesContainerSerializableComponents _servicesScopesContainer;
        [SerializeField] private SteamEditorConfig _steamEditorConfig;
        [SerializeField] private CursorConfig _cursorConfig;

        private IContainerBuilder _builder;

        protected override async void Awake()
        {
            autoRun = false;
            base.Awake();
            await this.CustomBuildAsync();
        }

        protected override async UniTask ConfigureAsync(IContainerBuilder builder)
        {
            _builder = builder;

            DontDestroyOnLoad(gameObject.transform.root);

            var transport = _steamEditorConfig.ShouldUseSteam ? _fishySteamworks : _unityTransport;
            var transportForDestroy = _steamEditorConfig.ShouldUseSteam ? _unityTransport : _fishySteamworks;

            Destroy(transportForDestroy);

            _networkManager.GetComponent<TransportManager>().Transport = transport;
            _networkManager.gameObject.SetActive(true);

            var assetLoader = AssetsLoaderTools.GetAssetLoader();
            
            RegisterSharedComponents();

            RegisterCoroutineRunner();
            RegisterMonoBehaviourObserver();
            
            await RegisterServicesAsync();
            
            await RegisterFactoriesAsync();
            
            await RegisterConfigsProviderServicesAsync();
            await RegisterGameStateMachineStatesAsync();
            _builder.RegisterEntryPoint<DependenciesCreator>();

            _builder.RegisterInstance(_servicesScopesContainer.MatchSharedServicesScope);
            _builder.RegisterInstance(_servicesScopesContainer.MatchServerServicesScope);
            _builder.RegisterInstance(_servicesScopesContainer.MatchClientServicesScope);

            _builder.RegisterInstance(new InputActions());
            _builder.Register<IInputProvider, NewInputSystemProvider>(Lifetime.Singleton);
            _builder.Register<GameStateMachine>(Lifetime.Singleton);
            _builder.RegisterEntryPoint<SubscribingMediator>();

            return;

            void RegisterCoroutineRunner()
            {
                var coroutineRunnerInstance = new GameObject("CoroutineRunner").AddComponent<CustomCoroutineRunner>();
                DontDestroyOnLoad(coroutineRunnerInstance);
                builder.RegisterComponent(coroutineRunnerInstance);
            }
        
            void RegisterMonoBehaviourObserver()
            {
                var monoBehaviourObserverInstance = new GameObject("GlobalMonoBehaviourObserver").AddComponent<MonoBehaviourObserver>();
                DontDestroyOnLoad(monoBehaviourObserverInstance);
                builder.RegisterComponent(monoBehaviourObserverInstance);
            }

            async UniTask RegisterFactoriesAsync()
            {
                await _builder.RegisterAllInheritorsAsync<IPersistentFactory>(Lifetime.Singleton);
            }

            async UniTask RegisterConfigsProviderServicesAsync()
            {
                await _builder.RegisterAllInheritorsAsync<IConfigsProviderService>(Lifetime.Singleton);
            }

            async UniTask RegisterGameStateMachineStatesAsync()
            {
                await _builder.RegisterAllInheritorsAsync<IGameState>(
                    Lifetime.Singleton);
            }

            async UniTask RegisterServicesAsync()
            {
                await _builder.RegisterAllInheritorsAsync<IPersistentService>(
                    Lifetime.Singleton);
            }

            void RegisterSharedComponents()
            {
                builder.RegisterInstance(builder);
                builder.RegisterInstance(assetLoader);
                builder.RegisterInstance(_steamEditorConfig);
                builder.RegisterInstance(_cursorConfig);
                builder.RegisterComponent(_networkManager);
                builder.RegisterComponent(_networkManager.ClientManager);
                builder.RegisterComponent(_networkManager.ServerManager);
                builder.RegisterComponent(_networkManager.SceneManager); ;
                builder.Register<HashedAssetProvider>(Lifetime.Transient);
            }
        }
    }
}