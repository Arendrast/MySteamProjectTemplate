using System;
using System.Buffers;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Pool
{
    public struct PooledArray<T> : IDisposable
    {
        public T[] Array { get; private set; }

        public static PooledArray<T> Get(int length)
        {
            return new PooledArray<T>()
            {
                Array = ArrayPool<T>.Shared.Rent(length)
            };
        }

        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(Array);
        }
    }
}