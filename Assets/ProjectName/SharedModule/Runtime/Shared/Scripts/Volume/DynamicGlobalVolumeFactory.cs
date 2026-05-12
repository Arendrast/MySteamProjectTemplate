using System;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using Object = UnityEngine.Object;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Volume
{
    public class DynamicGlobalVolumeFactory : IMatchSharedFactory, IDisposable
    {
        public DynamicGlobalVolumeSerializableComponents Volume { get; private set; }
        
        private readonly HashedAssetProvider _hashedAssetProvider;

        public DynamicGlobalVolumeFactory(HashedAssetProvider hashedAssetProvider)
        {
            _hashedAssetProvider = hashedAssetProvider;
        }

        public async UniTask<DynamicGlobalVolumeSerializableComponents> GetCreatedVolumeAsync()
        {
            Volume = await _hashedAssetProvider.GetOrLoadAndRegisterObjectAsync<DynamicGlobalVolumeSerializableComponents>("DynamicGlobalVolume");
            Object.DontDestroyOnLoad(Volume);
            return Volume;
        }

        public void Dispose()
        {
            _hashedAssetProvider?.Dispose();
        }
    }
}