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
    public class StopPlayable : PlayableAsset, ITimelineControllerPlayableAsset
    {
        [SerializeField] private StopBehaviour _template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<StopBehaviour>.Create(graph, _template);
        }

        [Serializable]
        public class StopBehaviour : MufflonUtil.PlayableBehaviour<TimelineController>
        {
            public enum Timing
            {
                Start,
                End
            }

            [SerializeField] private Timing _timing;

            protected override void OnBehaviourStart(TimelineController timelineController)
            {
                if(_timing == Timing.Start) timelineController.PlayableDirector.Stop();
            }

            protected override void OnBehaviourStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                if(_timing == Timing.End) timelineController.PlayableDirector.Stop();
            }
        }
    }
}