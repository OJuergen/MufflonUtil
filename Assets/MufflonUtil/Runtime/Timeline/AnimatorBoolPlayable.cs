using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class AnimatorBoolPlayable : AnimatorTrack.PlayableAsset<AnimatorBoolPlayable.PlayableBehaviour>
    {
        [Serializable]
        public class PlayableBehaviour : AnimatorTrack.PlayableBehaviour
        {
            [SerializeField] private string _parameter;
            [SerializeField] private bool _value;
            [SerializeField] private EndPolicy _endPolicy = EndPolicy.Previous;
            private bool _previousValue;
            private int _parameterHash;

            private enum EndPolicy
            {
                True,
                False,
                Previous
            }

            protected override void OnBehaviourUpdate(Playable playable, FrameData info, Animator playerData)
            {
                if (_parameterHash == 0)
                {
                    _parameterHash = Animator.StringToHash(_parameter);
                    if(Application.isPlaying) _previousValue = Context.GetBool(_parameterHash);
                }

                if(Application.isPlaying) Context.SetBool(_parameterHash, _value);
            }

            protected override void OnBehaviourStop(Playable playable, FrameData info, Animator playerData)
            {
                if (_endPolicy == EndPolicy.Previous && Context != null && Application.isPlaying)
                    Context.SetBool(_parameterHash, _previousValue);
                else if (_endPolicy == EndPolicy.True && Context != null && Application.isPlaying)
                    Context.SetBool(_parameterHash, true);
                else if (_endPolicy == EndPolicy.False && Context != null && Application.isPlaying)
                    Context.SetBool(_parameterHash, false);
                _parameterHash = 0;
            }
        }
    }
}