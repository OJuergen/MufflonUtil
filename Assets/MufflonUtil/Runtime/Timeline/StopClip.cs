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
    public class StopClip : TimelineControllerTrack.TimelineControllerClipAsset<StopClip.Behaviour>
    {
        [field: SerializeField] protected override Behaviour Template { get; set; }

        [Serializable]
        public class Behaviour : TimelineControllerTrack.ClipBehaviour
        {
            public enum Timing
            {
                Start,
                End
            }

            [SerializeField] private Timing _timing;

            protected override void OnStart(TimelineController timelineController)
            {
                if (_timing == Timing.Start) timelineController.PlayableDirector.Stop();
            }

            protected override void OnStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                if (_timing == Timing.End) timelineController.PlayableDirector.Stop();
            }
        }
    }
}