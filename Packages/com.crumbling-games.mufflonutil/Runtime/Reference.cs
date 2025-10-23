using MufflonUtil;
using UnityEngine;

namespace MufflonUtil
{
    public class Reference<T> : MonoBehaviour where T : Component
    {
        [SerializeField, SelfReference(true, true)] private T _component;
        public T Component => _component;
    }
}