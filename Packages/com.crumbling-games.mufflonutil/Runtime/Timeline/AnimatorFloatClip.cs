using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class AnimatorFloatClip : AnimatorTrack.Clip<AnimatorFloatClip.ClipBehaviour>
    {
        [SerializeField] private ClipBehaviour _behaviour;
        protected override ClipBehaviour BehaviourTemplate => _behaviour;

        [Serializable]
        public class ClipBehaviour : AnimatorTrack.AnimatorClipBehaviour
        {
            [SerializeField] private string _parameter;
            [SerializeField] private float _value;
            [SerializeField] private EndPolicy _endPolicy = EndPolicy.Keep;
            private float _previousValue;
            private int _parameterHash;

            public enum EndPolicy
            {
                Previous,
                Keep
            }

            protected override void OnUpdate(Playable playable, FrameData info, Animator playerData)
            {
                if (_parameterHash == 0)
                {
                    _parameterHash = Animator.StringToHash(_parameter);
                    _previousValue = Context.GetFloat(_parameterHash);
                }

                Context.SetFloat(_parameterHash, _value);
            }

            protected override void OnStop(Playable playable, FrameData info, Animator playerData)
            {
                if (_endPolicy == EndPolicy.Previous)
                {
                    Context.SetFloat(_parameterHash, _previousValue);
                }

                _parameterHash = 0;
            }
        }
    }
}