using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.PoolModule.Scripts
{
    public class ReleasableObjectPool<T> : IObjectPool<T> where T : MonoBehaviour
    {
        public IReadOnlyList<T> PoppedObjects => _poppedObjects;

        private readonly HashSet<T> _releasedObjects = new HashSet<T>();
        private readonly List<T> _poppedObjects = new List<T>();

        private readonly T _prefab;
        private readonly Action<T, int> _objectCreated;
        private readonly Action<T> _poppedObject;
        private readonly Action<T> _releasedObject;
        private readonly Action _initializedPool;
        private readonly int _size;
        private Transform _parent;
        
        private readonly int _numberObjectsToReleasePerFrame;

        public ReleasableObjectPool(string parentName, T prefab, Action<T, int> objectCreated = null,
            Action<T> poppedObject = null, Action<T> releasedObject = null, Action initializedPool = null,
            int size = 100, int numberObjectsToReleasePerFrame = 50)
        {
            _prefab = prefab;
            _objectCreated = objectCreated;
            _poppedObject = poppedObject;
            _releasedObject = releasedObject;
            _initializedPool = initializedPool;
            _size = size;
            _numberObjectsToReleasePerFrame = numberObjectsToReleasePerFrame;
            CreateParent(parentName);
            InitializePool();
        }

        public ReleasableObjectPool(Transform parent, T prefab, Action<T, int> objectCreated = null,
            Action<T> poppedObject = null, Action<T> releasedObject = null, Action initializedPool = null,
            int size = 100, int numberObjectsToReleasePerFrame = 50)
        {
            _prefab = prefab;
            _objectCreated = objectCreated;
            _poppedObject = poppedObject;
            _releasedObject = releasedObject;
            _initializedPool = initializedPool;
            _size = size;
            _parent = parent;
            _numberObjectsToReleasePerFrame = numberObjectsToReleasePerFrame;

            InitializePool();
        }

        public T PopUnprocessed()
        {
            var pooledObject = Dequeue();

            if (pooledObject == null)
                return null;

            pooledObject.gameObject.SetActive(true);
            OnPopCallback(pooledObject);

            return pooledObject;
        }

        public T PopProcessed(Vector3 position = default, Quaternion rotation = default, Transform parent = null,
            T @object = null)
        {
            if (position == default) position = Vector3.zero;
            if (rotation == default) rotation = Quaternion.identity;

            var pooledObject = Dequeue(@object);

            if (!pooledObject)
                return null;

            var transform = pooledObject.transform;

            transform.position = position;
            transform.rotation = rotation;

            transform.SetParent(parent);

            pooledObject.gameObject.SetActive(true);
            OnPopCallback(pooledObject);

            return pooledObject;
        }

        private T Dequeue(T @object = null)
        {
            if (@object && _releasedObjects.Remove(@object))
                return @object;

            if (_releasedObjects.Count <= 0) return InstantiateObject(0);

            var result = _releasedObjects.First();
            _releasedObjects.Remove(result);
            return result;
        }

        public async UniTask ReleaseAllPoppedObjectsAsync(bool withoutDelay = false)
        {
            var task = UniTask.DelayFrame(1);
            var counter = 0;
            
            for (var i = _poppedObjects.Count - 1; i >= 0; i--)
            {
                TryRelease(_poppedObjects[i]);

                if (withoutDelay)
                {
                    continue;
                }

                if (counter == _numberObjectsToReleasePerFrame)
                {
                    await task;
                    counter = 0;
                }
                else
                {
                    counter++;   
                }
            }
        }

        public bool TryReleaseRange(IEnumerable<T> objects)
        {
            var result = false;

            foreach (var @object in objects)
            {
                if (TryRelease(@object) && !result)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool TryRelease(T obj)
        {
            if (_releasedObjects.Contains(obj))
                return false;

            _poppedObjects.Remove(obj);
            _releasedObjects.Add(obj);
            obj.transform.SetParent(_parent);

            obj.gameObject.SetActive(false);
            OnReleaseCallback(obj);

            return true;
        }

        private void InitializePool()
        {
            for (var i = 0; i < _size; i++)
                _releasedObjects.Add(InstantiateObject(i));

            _initializedPool?.Invoke();
        }

        private void CreateParent(string parentName) => _parent = new GameObject(parentName + " Parent").transform;

        private T InstantiateObject(int index)
        {
            var instance = Object.Instantiate(_prefab, _parent);
            instance.gameObject.SetActive(false);
            OnCreate(instance, index);
            return instance;
        }

        private void OnCreate(T createdObject, int index) => _objectCreated?.Invoke(createdObject, index);

        private void OnPopCallback(T givenObject)
        {
            _poppedObject?.Invoke(givenObject);
            _poppedObjects.Add(givenObject);
        }

        private void OnReleaseCallback(T releasedObject)
        {
            _releasedObject?.Invoke(releasedObject);
        }
    }
}
