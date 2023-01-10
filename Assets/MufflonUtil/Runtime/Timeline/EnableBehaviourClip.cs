using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [DisplayName("Enable Behaviour")]
    public class EnableBehaviourClip : BehaviourTrack.BehaviourClipAsset<EnableBehaviourClip.Behaviour>
    {
        [field:SerializeField] protected override Behaviour Template { get; set; }

        [Serializable]
        public class Behaviour : BehaviourTrack.ClipBehaviour
        {
            [SerializeField] private PostPlaybackBehaviour _postPlaybackBehaviour;
            [SerializeField] private bool _isEnabled;
            private bool _wasEnabled;

            protected override void OnStart(UnityEngine.Behaviour behaviour)
            {
                _wasEnabled = behaviour.enabled;
                behaviour.enabled = _isEnabled;
            }

            protected override void OnStop(Playable playable, FrameData info, UnityEngine.Behaviour behaviour)
            {
                switch (_postPlaybackBehaviour)
                {
                    case PostPlaybackBehaviour.Revert:
                        behaviour.enabled = _wasEnabled;
                        break;
                    case PostPlaybackBehaviour.Active:
                        behaviour.enabled = true;
                        break;
                    case PostPlaybackBehaviour.Inactive:
                        behaviour.enabled = false;
                        break;
                    case PostPlaybackBehaviour.KeepAsIs:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [Serializable]
        public enum PostPlaybackBehaviour
        {
            KeepAsIs,
            Revert,
            Active,
            Inactive
        }
    }
}