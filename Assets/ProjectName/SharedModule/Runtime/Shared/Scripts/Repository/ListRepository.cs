using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectName.SharedModule.Runtime.Shared.Scripts.QoL;
using VContainer;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Repository
{
    public class ListRepository<T> : IRepository, IList<T>, IReadOnlyList<T>
    {
        public int Count => _list.Count;
        public bool IsReadOnly => ((IList<T>)_list).IsReadOnly;

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public event Action<T> Added, Removed;

        private readonly List<T> _list = new List<T>();

        [Inject]
        public ListRepository()
        {
        }

        public ListRepository(ListRepository<T> repository)
        {
            _list = repository.ToList();
        }

        public void Dispose()
        {
            _list.Clear();
        }

        public IDisposable SubscribeOnAdded(Action<T> action)
        {
            foreach (var value in _list)
            {
                action.Invoke(value);
            }

            Added += action;
            return new InvokeOnDispose(() => Added -= action);
        }
        
        public IDisposable SubscribeOnRemove(Action<T> action)
        {
            Removed += action;
            return new InvokeOnDispose(() => Removed -= action);
        }

        public void Add(T value)
        {
            _list.Add(value);
            Added?.Invoke(value);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T value)
        {
            if (_list.Remove(value))
            {
                Removed?.Invoke(value);
                return true;
            }

            return false;
        }

        public void RemoveAll(Predicate<T> func)
        {
            var oldListCopy = _list.ToList();
            _list.RemoveAll(func);
            var currentListCopy = _list.ToList();

            foreach (var element in oldListCopy.Except(currentListCopy))
            {
                Removed?.Invoke(element);
            }
        }

        public void Clear()
        {
            foreach (var element in _list)
            {
                Removed?.Invoke(element);
            }

            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        bool ICollection<T>.Remove(T item)
        {
            return Remove(item);
        }
    }
}