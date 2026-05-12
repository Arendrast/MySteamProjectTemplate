using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class ReflectionTools
    {
        public static async UniTask<IEnumerable<Type>> GetAllInheritorsTypesAsync<T>(bool includeAbstractClasses)
        {
            return (await LoadingTypeTools.GetLoadedTypesAsync())
                .Where(typePair =>
                    typeof(T).IsAssignableFrom(typePair.Value) &&
                    !typePair.Value.IsInterface &&
                    (!includeAbstractClasses || typePair.Value.IsAbstract)).Select(pair => pair.Value);
        }
    }
}