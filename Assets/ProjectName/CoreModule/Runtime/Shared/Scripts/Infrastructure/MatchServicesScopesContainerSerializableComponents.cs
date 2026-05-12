using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class MatchServicesScopesContainerSerializableComponents : SerializedMonoBehaviour
    {
        [field: SerializeField] public IMatchServerServicesScope MatchServerServicesScope { get; private set; }
        [field: SerializeField] public IMatchClientServicesScope MatchClientServicesScope { get; private set; }
        [field: SerializeField] public MatchSharedServicesScope MatchSharedServicesScope { get; private set; }
    }
}