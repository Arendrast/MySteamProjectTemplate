using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Tools;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Serialization
{
    public static class JsonTools
    {
        private static bool _doesLoadSettings;
        private static JsonSerializerSettings _generalJsonSerializerSettings;

        private static async UniTask<JsonSerializerSettings> GetJsonSerializerSettingsAsync()
        {
            if (_generalJsonSerializerSettings != null)
            {
                return _generalJsonSerializerSettings;
            }

            if (_doesLoadSettings)
            {
                await UniTask.WaitWhile(() => _generalJsonSerializerSettings == null);
                return _generalJsonSerializerSettings;
            }

            _doesLoadSettings = true;
            
            return _generalJsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                SerializationBinder = new KnownTypesBinder(await LoadingTypeTools.GetLoadedTypesAsync())
            };
        }

        public static async UniTask<string> GetJsonSerializedObjectWithoutNullsAsync(this object obj, TypeNameHandling typeNameHandling)
        {
            var settings = await GetJsonSerializerSettingsAsync();
            settings.TypeNameHandling = typeNameHandling;
            var result = JsonConvert.SerializeObject(obj, settings);
            settings.TypeNameHandling = TypeNameHandling.Auto;

            return result;
        }

        public static async UniTask<T> GetFromJsonDeserializedWithoutNullsAsync<T>(this string str) =>
            JsonConvert.DeserializeObject<T>(str, await GetJsonSerializerSettingsAsync());

        public static async UniTask<object> GetFromJsonDeserializedWithoutNullsAsync(this string str) =>
            str == null ? null : JsonConvert.DeserializeObject(str, await GetJsonSerializerSettingsAsync());
    }
}