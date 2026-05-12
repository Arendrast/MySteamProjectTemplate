using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Serialization
{
    public class KnownTypesBinder : ISerializationBinder
    {
        public IReadOnlyDictionary<string, Type> KnownTypesByName { get; }

        public KnownTypesBinder(IReadOnlyDictionary<string, Type> knwonTypesByName)
        {
            KnownTypesByName = knwonTypesByName;
        }

        public Type BindToType(string assemblyName, string typeName)
            => KnownTypesByName.GetValueOrDefault(typeName);

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.FullName;
        }
    }
}