using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Configs;
using Object = UnityEngine.Object;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Index
{
    public class IndexableConfigsProviderService<TMainConfig, TIndexableConfig> : IConfigsProviderService
        where TMainConfig : Object, IIndexableConfigsProvider<TIndexableConfig> where TIndexableConfig : IIndexable
    {
        private readonly ConfigsProviderService _configsProviderService;

        public IndexableConfigsProviderService(ConfigsProviderService configsProviderService)
        {
            _configsProviderService = configsProviderService;
        }

        public async UniTask<TIndexableConfig> GetConfigAsync(int id)
        {
            return (await _configsProviderService.GetConfigAsync<TMainConfig>()).Configs.FirstOrDefault(config =>
                config?.Id == id);
        }
    }
}