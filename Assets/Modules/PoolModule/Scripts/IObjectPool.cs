using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.PoolModule.Scripts
{
    public interface IObjectPool<T> : IReleasableObjectPool where T : MonoBehaviour
    {
        IReadOnlyList<T> PoppedObjects { get; }
        T PopProcessed(Vector3 position = default, Quaternion rotation = default, Transform parent = null, T @object = null);
        T PopUnprocessed();
        bool TryRelease(T releasingObject);
        bool TryReleaseRange(IEnumerable<T> objects);
    }
}