using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.SharedModule.Runtime.Shared.Scripts.Index;
using Modules.SharedModule.Runtime.Shared.Scripts.Tools;

namespace Modules.SharedModule.Runtime.Shared.Scripts.Repository
{
    public abstract class IndexRepository<TKey, TValue> : IRepository, IDisposable
    {
        public IReadOnlyDictionary<TKey, TValue> ValueByKey => _valueByKey;
        public IReadOnlyDictionary<TValue, TKey> KeyByValue => _keyByValue;

        public event Action<TKey, TValue> Added, Removed;

        private CancellationTokenSource _disposeTokenSource = new CancellationTokenSource();
        private readonly Dictionary<TKey, TValue> _valueByKey = new();
        private readonly Dictionary<TValue, TKey> _keyByValue = new();

        public void Dispose()
        {
            _valueByKey.Clear();
            _keyByValue.Clear();

            _disposeTokenSource.Cancel();
            _disposeTokenSource.Dispose();
            _disposeTokenSource = new CancellationTokenSource();
        }

        public async UniTask<TValue> GetValueByKeyOrWaitUntilAddAsync(TKey key, float maxWaitTimeInSeconds = 5)
        {
            TValue value = default;

            var token = CancellationTokenSource.CreateLinkedTokenSource(_disposeTokenSource.Token,
                new CancellationTokenSource(TimeSpan.FromSeconds(maxWaitTimeInSeconds)).Token).Token;
            
            if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                    UniTask.WaitWhile(() => !_valueByKey.TryGetValue(key, out value),
                        cancellationToken: token)))
            {
                return default;
            }

            return value;
        }

        public async UniTask<TKey> GetValueByKeyOrWaitUntilAddAsync(TValue value, float maxWaitTimeInSeconds = 5)
        {
            TKey key = default;
            
            var token = CancellationTokenSource.CreateLinkedTokenSource(_disposeTokenSource.Token,
                new CancellationTokenSource(TimeSpan.FromSeconds(maxWaitTimeInSeconds)).Token).Token;

            if (await AsyncTools.AwaitTaskAndGetDoesThrowOperationCancelledException(
                    UniTask.WaitWhile(() => !_keyByValue.TryGetValue(value, out key),
                        cancellationToken: token)))
            {
                return default;
            }

            return key;
        }

        public void Add(TKey key, TValue value)
        {
            if (!_valueByKey.TryAdd(key, value))
            {
                throw new ArgumentException(
                    $"An item with the same key has already been added. key = {key} value = {value}");
            }

            _keyByValue.Add(value, key);

            Added?.Invoke(key, value);
        }

        public void Add<T>(T value) where T : TValue, IIndexable<TKey>
        {
            Add(value.Id, value);
        }

        public void TryAdd(TKey key, TValue value)
        {
            try
            {
                Add(key, value);
            }
            catch
            {
            }
        }

        public bool RemoveByValue(TValue value)
        {
            if (!_keyByValue.Remove(value, out var key))
            {
                return false;
            }

            _valueByKey.Remove(key);
            Removed?.Invoke(key, value);
            return true;
        }


        public bool RemoveByKey(TKey key)
        {
            if (!_valueByKey.Remove(key, out var value))
            {
                return false;
            }

            _keyByValue.Remove(value);
            Removed?.Invoke(key, value);
            return true;
        }

        public bool ContainsKey(TKey key)
        {
            return ValueByKey.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return KeyByValue.ContainsKey(value);
        }

        public TValue GetValueOrDefault(TKey key)
        {
            return ValueByKey.GetValueOrDefault(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return ValueByKey.TryGetValue(key, out value);
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            return KeyByValue.TryGetValue(value, out key);
        }
    }
}