using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [DisplayName("Timeline Control/Jump Clip")]
    public class JumpPlayable : PlayableAsset, ITimelineControllerPlayableAsset
    {
        [SerializeField] private JumpBehaviour _template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<JumpBehaviour>.Create(graph, _template);
        }

        [Serializable]
        public class JumpBehaviour : MufflonUtil.PlayableBehaviour<TimelineController>
        {
            private bool _isInitialized;
            [SerializeField, Tooltip("Name of JumpMarker that marks the jump target time")]
            private string _target;

            public void Jump()
            {
                JumpMarker targetMarker = (Context.PlayableDirector.playableAsset as TimelineAsset)?.GetOutputTracks()
                    .SelectMany(t => t.GetMarkers())
                    .OfType<JumpMarker>()
                    .FirstOrDefault(m => m.name.Equals(_target));
                if (targetMarker == null)
                {
                    Debug.LogWarning($"No matching target JumpMarker with name {_target} found!");
                    return;
                }

                Context.PlayableDirector.Pause(); // prevent markers that are jumped from being executed
                Context.PlayableDirector.time = targetMarker.time;
                Context.PlayableDirector.Play();
            }

            protected override void OnBehaviourStart(TimelineController timelineController)
            { }

            protected override void OnBehaviourUpdate(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                if (!_isInitialized)
                {
                    timelineController.Jumping += Jump;
                    _isInitialized = true;
                }
            }

            protected override void OnBehaviourStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                _isInitialized = false;
                timelineController.Jumping -= Jump;
            }
        }
    }
}