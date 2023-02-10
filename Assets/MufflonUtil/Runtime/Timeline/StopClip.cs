using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    /// <summary>
    /// Clip that stops the <see cref="PlayableDirector"/>.
    /// </summary>
    [DisplayName("Timeline Control/Stop Timeline Clip")]
    public class StopClip : TimelineControllerTrack.Clip<StopClip.ClipBehaviour>
    {
        private enum Timing
        {
            Start,
            End
        }

        [field: SerializeField] private Timing StopTiming { get; set; }

        [Serializable]
        public class ClipBehaviour : TimelineControllerTrack.ClipBehaviour
        {
            private StopClip StopClip => ClipAsset as StopClip;

            protected override void OnStart(TimelineController timelineController)
            {
                if (StopClip.StopTiming == Timing.Start) timelineController.PlayableDirector.Stop();
            }

            protected override void OnStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                if (StopClip.StopTiming == Timing.End) timelineController.PlayableDirector.Stop();
            }
        }
    }
}