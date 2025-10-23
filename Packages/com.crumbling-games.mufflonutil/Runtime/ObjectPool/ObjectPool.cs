using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MufflonUtil
{
    public class ObjectPool<T> : IDisposable where T : MonoBehaviour, IPooledObject<T>
    {
        private Queue<T> _pool = new Queue<T>();
        private T _prefab;
        private Transform _container;
        private bool _initialized;

        public void Init(T prefab, Transform container = null)
        {
            _prefab = prefab;
            _container = container;
            _initialized = true;
        }

        public T Get()
        {
            if (!_initialized)
                throw new InvalidOperationException("Pool not initialized");

            while (_pool.Count > 0)
            {
                T item = _pool.Dequeue();
                item.gameObject.SetActive(true);
                return item;
            }

            return CreateItem();
        }

        public void Grow(int count)
        {
            if (!_initialized)
                throw new InvalidOperationException("Pool not initialized");
            for (var i = 0; i < count; i++)
            {
                ReturnToPool(CreateItem());
            }
        }

        private T CreateItem()
        {
            T item = Object.Instantiate(_prefab, _container);
            item.Pool = this;
            item.EventTrigger().Destroyed += OnItemDestroyed;
            return item;
        }

        private void OnItemDestroyed(EventTrigger obj)
        {
            var destroyedItem = obj.GetComponent<T>();
            _pool = new Queue<T>(_pool.Where(item => item != destroyedItem));
        }

        public void ReturnToPool(T item)
        {
            if (!_initialized)
                throw new InvalidOperationException("Pool not initialized");
            _pool.Enqueue(item);
            item.transform.parent = _container;
            item.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            foreach (T item in _pool)
            {
                item.EventTrigger().Destroyed -= OnItemDestroyed;
            }
        }
    }
}