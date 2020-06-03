using UnityEngine;

namespace MufflonUtil
{
    [CreateAssetMenu(fileName = "ObjectPool", menuName = "Util/Object Pool")]
    public class ObjectPoolAsset : ObjectPoolAsset<PooledObject>
    { }

    public class ObjectPoolAsset<TItem> : ScriptableObject where TItem : MonoBehaviour, IPooledObject<TItem>
    {
        [SerializeField] private TItem _prefab;
        public ObjectPool<TItem> Pool { get; private set; }

        private void OnEnable()
        {
            Pool = new ObjectPool<TItem>();
            Pool.Init(_prefab);
        }

        private void OnDisable()
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