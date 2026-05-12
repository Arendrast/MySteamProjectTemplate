using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.QoL;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;
using Object = UnityEngine.Object;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Configs
{
    public sealed class ConfigsProviderService : IPersistentService, IDisposable
    {
        private readonly HashedAssetProvider _hashedAssetProvider;

        public ConfigsProviderService(HashedAssetProvider hashedAssetProvider)
        {
            _hashedAssetProvider = hashedAssetProvider;
        }

        public void Dispose()
        {
            DisposeAsync().Forget();
        }

        public async UniTask DisposeAsync()
        {
            await _hashedAssetProvider.DisposeAsync();
        }

        public async UniTask<TConfig> GetConfigAsync<TConfig>() where TConfig: Object
        {
            var scope = typeof(TConfig).GetCustomAttribute<ConfigScopeAttribute>()?.Scope;
            
            if (scope.IsNullOrEmptyOrWhiteSpace())
            {
                throw new Exception("Config scope is null or empty or white space");
            }
            
            return await _hashedAssetProvider.GetOrLoadAndRegisterObjectAsync<TConfig>(scope, shouldCheckIsPlaying: false);
        }
    }
}