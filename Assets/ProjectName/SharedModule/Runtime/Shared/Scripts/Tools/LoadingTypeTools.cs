using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Tools
{
    public static class LoadingTypeTools
    {
        private static bool _doesLoad;
        private static IReadOnlyDictionary<string, Type> _cachedTypes;

        private const int MaximumHandleAssembliesPerFrame = 10; // Arendrast: you can increase

        public static async UniTask<IReadOnlyDictionary<string, Type>> GetLoadedTypesAsync()
        {
            if (_cachedTypes != null)
            {
                return _cachedTypes;
            }

            if (_doesLoad)
            {
                await UniTask.WaitWhile(() => _cachedTypes == null);
                return _cachedTypes;
            }

            _doesLoad = true;

            var dictionary = new Dictionary<string, Type>(8192); // Arendrast: Predicted size for minimize reallocations
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var counter = 0;

            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        var typeName = type.FullName;
                        if (!dictionary.TryAdd(typeName, type))
                        {
                            
                        }
                    }

                    counter++;

                    if (counter % MaximumHandleAssembliesPerFrame == 0)
                        await UniTask.DelayFrame(1);
                }
                catch (ReflectionTypeLoadException)
                {
                    continue;
                }
            }

            return _cachedTypes = dictionary;
        }

        public static Type GetTypeByName(string name)
        {
            if (_cachedTypes != null && _cachedTypes.TryGetValue(name, out var type))
                return type;
            return null;
        }
    }
}