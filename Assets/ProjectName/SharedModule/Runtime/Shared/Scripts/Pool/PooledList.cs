using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Pool
{
    public struct PooledList<T> : IDisposable
    {
        public List<T> List { get; private set; }

        public static PooledList<T> Get()
        {
            var list = ListPool<T>.Get();
            return new PooledList<T>
            {
                List = list
            };
        }

        public void Dispose()
        {
            ListPool<T>.Release(List);
        }
    }
}