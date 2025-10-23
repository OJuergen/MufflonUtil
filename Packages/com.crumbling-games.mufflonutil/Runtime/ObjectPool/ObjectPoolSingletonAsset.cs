using UnityEngine;

namespace MufflonUtil
{
    public class ObjectPoolSingletonAsset<TItem, TPool> : ScriptableObjectSingleton<TPool> 
        where TItem : MonoBehaviour, IPooledObject<TItem>
        where TPool : ObjectPoolSingletonAsset<TItem, TPool>
    {
        [SerializeField] private TItem _prefab;
        public ObjectPool<TItem> Pool { get; private set; }

        private new void OnEnable()
        {
            base.OnEnable();
            Pool = new ObjectPool<TItem>();
            Pool.Init(_prefab);
        }

        private new void OnDisable()
        {
            base.OnDisable();
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