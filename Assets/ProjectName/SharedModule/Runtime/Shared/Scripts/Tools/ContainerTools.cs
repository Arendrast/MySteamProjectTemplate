using System;
using Cysharp.Threading.Tasks;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using UnityEngine;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class ContainerTools
    {
        public static async UniTask<TController> GetControllerAsync<TController, TView>(
            this HashedAssetProvider hashedAssetProvider,
            string viewAssetId,
            Func<TView, UniTask> created,
            bool shouldMakeDontDestroyOnLoad = false,
            Transform parent = null,
            bool shouldAppointParentAfterInstantiate = false) where TView : MonoBehaviour where TController : class
        {
            var viewInstance = await hashedAssetProvider.GetOrLoadAndRegisterObjectAsync(
                viewAssetId, 
                created,
                shouldCreate: true,
                parent: parent,
                shouldMakeDontDestroyOnLoad: shouldMakeDontDestroyOnLoad,
                shouldAppointParentAfterInstantiate: shouldAppointParentAfterInstantiate);

            return hashedAssetProvider.GetSingleByType<TController>();
        }
    }
}