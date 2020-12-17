using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [DisplayName("Enable Behaviour")]
    public class EnableBehaviourPlayable : BehaviourTrack.PlayableAsset<EnableBehaviourPlayable.PlayableBehaviour>
    {
        [Serializable]
        public class PlayableBehaviour : BehaviourTrack.Behaviour
        {
            [SerializeField] private PostPlaybackBehaviour _postPlaybackBehaviour;
            [SerializeField] private bool _isEnabled;
            private bool _wasEnabled;

            protected override void OnBehaviourStart(Behaviour behaviour)
            {
                _wasEnabled = behaviour.enabled;
                behaviour.enabled = _isEnabled;
            }

            protected override void OnBehaviourStop(Playable playable, FrameData info, Behaviour behaviour)
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