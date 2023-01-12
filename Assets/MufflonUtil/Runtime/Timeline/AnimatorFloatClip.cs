using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class FloatClip : AnimatorTrack.Clip<FloatClip.Behaviour>
    {
        [field:SerializeField] protected override Behaviour Template { get; set; }

        [Serializable]
        public class Behaviour : AnimatorTrack.AnimatorBehaviour
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