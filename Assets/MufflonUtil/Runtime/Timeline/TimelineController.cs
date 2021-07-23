using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineController : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _playableDirector;
        public PlayableDirector PlayableDirector => _playableDirector;
        
        public event Action BreakingLoop;
        public event Action Jumping;

        [ContextMenu("Break Loop")]
        public void BreakLoop()
        {
            BreakingLoop?.Invoke();
        }

        [ContextMenu("Jump")]
        public void Jump()
        {
            Jumping?.Invoke();
        }
    }
}