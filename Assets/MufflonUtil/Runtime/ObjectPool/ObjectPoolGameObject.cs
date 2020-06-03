using UnityEngine;

namespace MufflonUtil
{
    public class ObjectPoolGameObject<TItem> : MonoBehaviour where TItem : MonoBehaviour, IPooledObject<TItem>
    {
        [SerializeField] private TItem _prefab;
        [SerializeField] private int _preWarm;
        public ObjectPool<TItem> Pool { get; private set; }

        private void Awake()
        {
            Pool = new ObjectPool<TItem>();
            Pool.Init(_prefab, transform);
            Pool.Grow(_preWarm);
        }

        private void OnDestroy()
        {
            Pool.Dispose();
        }

        public void Add(int count)
        {
            Pool.Grow(count);
        }

        public void Return(TItem t)
        {
            Pool.ReturnToPool(t);
        }

        public TItem Get()
        {
            return Pool.Get();
        }
    }
}