using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [DisplayName("Enable Behaviour")]
    public class EnableBehaviourClip : BehaviourTrack.Clip<EnableBehaviourClip.ClipBehaviour>
    {
        [field: SerializeField] private PostPlaybackBehaviour PostPlaybackBehaviour { get; set; }
        [field: SerializeField] private bool IsEnabled { get; set; }
        [field: SerializeField] private bool ExecuteInEditMode { get; set; }

        [Serializable]
        public class ClipBehaviour : TimelineTrack<Behaviour>.ClipBehaviour
        {
            private EnableBehaviourClip EnableClip => ClipAsset as EnableBehaviourClip;
            private bool _wasEnabled;

            protected override void OnStart(Behaviour behaviour)
            {
                _wasEnabled = behaviour.enabled;
                if (Application.isPlaying || EnableClip.ExecuteInEditMode)
                {
                    behaviour.enabled = EnableClip.IsEnabled;
                }
            }

            protected override void OnStop(Playable playable, FrameData info, Behaviour behaviour)
            {
                if (Application.isPlaying || EnableClip.ExecuteInEditMode)
                {
                    switch (EnableClip.PostPlaybackBehaviour)
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
        }
    }
}