using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [DisplayName("Timeline Control/Loop Clip")]
    public class LoopPlayable : PlayableAsset, ITimelineControllerPlayableAsset
    {
        [SerializeField] private LoopBehaviour _template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<LoopBehaviour>.Create(graph, _template);
        }

        [Serializable]
        public class LoopBehaviour : PlayableBehaviour<TimelineController>
        {
            private float _time;
            private bool _isInitialized;
            private bool _broken;

            public void Break()
            {
                _broken = true;
            }

            protected override void OnBehaviourStart(TimelineController playerData)
            {
                _isInitialized = false;
            }

            protected override void OnBehaviourUpdate(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                if (!_isInitialized)
                {
                    _time = (float) timelineController.PlayableDirector.time;
                    _isInitialized = true;
                    timelineController.BreakingLoop += Break;
                }
            }

            protected override void OnBehaviourStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                PlayableDirector playableDirector = timelineController.PlayableDirector;
                if (playableDirector == null) return;
                bool inClip = playableDirector.time - info.effectiveSpeed * info.deltaTime >= _time &&
                              playableDirector.time - info.effectiveSpeed * info.deltaTime <= _time + playable.GetDuration();
                // Debug.Log($"Loop clip stopped: isBroken {_broken}, director state: {playableDirector.state}, inClip: {inClip}, deltaTime: {info.deltaTime}");
                if (!_broken && playableDirector.state == PlayState.Playing && inClip) playableDirector.time = _time;
                else timelineController.BreakingLoop -= Break;
            }
        }
    }
}