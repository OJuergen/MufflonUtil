using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class BoolClip : AnimatorTrack.Clip<BoolClip.ClipBehaviour>
    {
        [SerializeField] private ClipBehaviour _behaviour;
        protected override ClipBehaviour BehaviourTemplate => _behaviour;

        [Serializable]
        public class ClipBehaviour : AnimatorTrack.AnimatorClipBehaviour
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

            protected override void OnUpdate(Playable playable, FrameData info, Animator playerData)
            {
                if (_parameterHash == 0)
                {
                    _parameterHash = Animator.StringToHash(_parameter);
                    if (Application.isPlaying) _previousValue = Context.GetBool(_parameterHash);
                }

                if (Application.isPlaying) Context.SetBool(_parameterHash, _value);
            }

            protected override void OnStop(Playable playable, FrameData info, Animator playerData)
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