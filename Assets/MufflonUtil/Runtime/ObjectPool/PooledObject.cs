using UnityEngine;

namespace MufflonUtil
{
    public class PooledObject : MonoBehaviour, IPooledObject<PooledObject>
    {
        [SerializeField, Tooltip("If larger than zero, the object will automatically be returned to the object pool " +
                                 "after this period of time.")]
        private float _lifeTimeSeconds;
        public ObjectPool<PooledObject> Pool { get; set; }

        private void OnEnable()
        {
            if (_lifeTimeSeconds > 0) Invoke(nameof(ReturnToPool), _lifeTimeSeconds);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void ReturnToPool()
        {
            Pool.ReturnToPool(this);
        }
    }
}