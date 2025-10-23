using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MufflonUtil
{
    /// <summary>
    /// A component implementation of the <see cref="IBlockable"/> interface, also offering serializable events.
    /// Uses a <see cref="HashSet{T}"/> internally to keep track of blockers.
    /// </summary>
    public class Blockable : MonoBehaviour, IBlockable
    {
        private readonly HashSet<object> _blockers = new HashSet<object>();
        public event Action Blocked;
        public event Action Unblocked;
        [SerializeField] private UnityEvent _blocked;
        [SerializeField] private UnityEvent _unblocked;
        public bool IsBlocked => _blockers.Any(blocker => blocker != null);

        public void AddBlocker(object blocker)
        {
            if (_blockers.Contains(blocker)) return;
            _blockers.RemoveWhere(b => b == null);
            bool wasBlockedBefore = IsBlocked;
            _blockers.Add(blocker);
            if (!wasBlockedBefore)
            {
                Blocked?.Invoke();
                _blocked?.Invoke();
            }
        }

        public void RemoveBlocker(object blocker)
        {
            if (!_blockers.Contains(blocker)) return;
            _blockers.RemoveWhere(b => b == null);
            _blockers.Remove(blocker);
            if (!IsBlocked)
            {
                Unblocked?.Invoke();
                _unblocked?.Invoke();
            }
        }
    }
}