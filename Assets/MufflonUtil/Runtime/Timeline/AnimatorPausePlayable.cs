using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class AnimatorPausePlayable : AnimatorTrack.PlayableAsset<AnimatorPausePlayable.PlayableBehaviour>
    {
        [Serializable]
        public class PlayableBehaviour : AnimatorTrack.PlayableBehaviour
        {
            [SerializeField] private EndPolicy _endPolicy = EndPolicy.Previous;
            private bool _previousValue;

            public enum EndPolicy
            {
                Previous,
                Keep
            }

            protected override void OnBehaviourStart(Animator animator)
            {
                _previousValue = Animator.enabled;
                if(Application.isPlaying) Animator.enabled = false;
            }

            protected override void OnBehaviourStop(Playable playable, FrameData info, Animator animator)
            {
                if (Application.isPlaying && _endPolicy == EndPolicy.Previous)
                    animator.enabled = _previousValue;
            }
        }
    }
}