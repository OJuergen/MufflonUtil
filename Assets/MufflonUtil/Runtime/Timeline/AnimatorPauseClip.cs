using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class PauseClip : AnimatorTrack.Clip<PauseClip.Behaviour>
    {
        [field:SerializeField] protected override Behaviour Template { get; set; }

        [Serializable]
        public class Behaviour : AnimatorTrack.AnimatorBehaviour
        {
            [SerializeField] private EndPolicy _endPolicy = EndPolicy.Previous;
            private bool _previousValue;

            public enum EndPolicy
            {
                Previous,
                Keep
            }

            protected override void OnStart(Animator animator)
            {
                _previousValue = Animator.enabled;
                if (Application.isPlaying) Animator.enabled = false;
            }

            protected override void OnStop(Playable playable, FrameData info, Animator animator)
            {
                if (Application.isPlaying && _endPolicy == EndPolicy.Previous)
                    animator.enabled = _previousValue;
            }
        }
    }
}