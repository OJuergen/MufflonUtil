using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [DisplayName("Timeline Control/Jump")]
    public class JumpClip : TimelineControllerTrack.Clip<JumpClip.Behaviour>
    {
        [field:SerializeField] protected override Behaviour Template { get; set; }

        [Serializable]
        public class Behaviour : TimelineControllerTrack.Behaviour
        {
            private bool _isInitialized;
            [SerializeField, MarkerFromTimeline] private JumpMarker _marker;
            
            public void Jump()
            {
                if (_marker == null)
                {
                    Debug.LogWarning($"No matching target JumpMarker {_marker} found!");
                    return;
                }

                Context.BreakLoop();
                Context.PlayableDirector.Pause(); // prevent markers that are jumped from being executed
                Context.PlayableDirector.time = _marker.time;
                Context.PlayableDirector.Play();
            }

            protected override void OnStart(TimelineController timelineController)
            { }

            protected override void OnUpdate(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                if (!_isInitialized)
                {
                    timelineController.Jumping += Jump;
                    _isInitialized = true;
                }
            }

            protected override void OnStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                _isInitialized = false;
                timelineController.Jumping -= Jump;
            }
        }
    }
}