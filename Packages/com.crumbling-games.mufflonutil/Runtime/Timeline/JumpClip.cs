using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [DisplayName("Timeline Control/Jump")]
    public class JumpClip : TimelineControllerTrack.Clip<JumpClip.ClipBehaviour>
    {
        [SerializeField, MarkerFromTimeline] private JumpMarker _marker;

        public class ClipBehaviour : TimelineTrack<TimelineController>.ClipBehaviour
        {
            private JumpClip JumpClip => ClipAsset as JumpClip;
            private bool _isInitialized;

            public void Jump()
            {
                if (JumpClip._marker == null)
                {
                    Debug.LogWarning($"No matching target JumpMarker {JumpClip._marker} found!");
                    return;
                }

                Context.BreakLoop();
                Context.PlayableDirector.Pause(); // prevent markers that are jumped from being executed
                Context.PlayableDirector.time = JumpClip._marker.time;
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