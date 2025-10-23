using UnityEngine;

namespace MufflonUtil
{
    public interface IPooledObject<T> where T : MonoBehaviour, IPooledObject<T>
    {
        ObjectPool<T> Pool { get; set; }
    }
}