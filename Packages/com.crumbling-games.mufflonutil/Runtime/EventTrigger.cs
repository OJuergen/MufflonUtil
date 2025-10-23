using System;
using UnityEngine;

namespace MufflonUtil
{
    public class EventTrigger : MonoBehaviour
    {
        public event Action<EventTrigger> Enabled;
        public event Action<EventTrigger> Disabled;
        public event Action<EventTrigger> Awoke;
        public event Action<EventTrigger> Started;
        public event Action<EventTrigger> Destroyed;

        private void Awake()
        {
            Awoke?.Invoke(this);
        }

        private void OnEnable()
        {
            Enabled?.Invoke(this);
        }

        private void OnDisable()
        {
            Disabled?.Invoke(this);
        }

        private void Start()
        {
            Started?.Invoke(this);
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}