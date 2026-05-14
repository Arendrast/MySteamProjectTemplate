using Cysharp.Threading.Tasks;

namespace Modules.PoolModule.Scripts
{
    public interface IReleasableObjectPool
    {
        UniTask ReleaseAllPoppedObjectsAsync(bool withoutDelay = false);
    }
}