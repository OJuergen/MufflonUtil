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
            private DirectorWrapMode _directorWrapMode;
            private PlayableAsset _playableAsset;

            public void Break()
            {
                _broken = true;
            }

            protected override void OnBehaviourStart(TimelineController timelineController)
            {
                PlayableDirector playableDirector = timelineController.PlayableDirector;
                _playableAsset = playableDirector.playableAsset;
                _isInitialized = false;
                _directorWrapMode = playableDirector.extrapolationMode;
                // prevent director from stopping when timeline ends with loop
                playableDirector.extrapolationMode = DirectorWrapMode.Hold;
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

                bool isOutOfClipNextFrame = timelineController.PlayableDirector.time - _time + info.deltaTime > playable.GetDuration();
                if(!_broken && isOutOfClipNextFrame) timelineController.PlayableDirector.time = _time;
            }

            protected override void OnBehaviourStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                PlayableDirector playableDirector = timelineController.PlayableDirector;
                if (playableDirector == null) return;
                bool inClip = playableDirector.time - info.effectiveSpeed * info.deltaTime >= _time &&
                              playableDirector.time - info.effectiveSpeed * info.deltaTime <= _time + playable.GetDuration();
                // Debug.Log($"Loop clip stopped: isBroken {_broken}, director state: {playableDirector.state}, inClip: {inClip}, deltaTime: {info.deltaTime}");
                if (!_broken && playableDirector.state == PlayState.Playing &&
                    playableDirector.playableAsset == _playableAsset)
                {
                    playableDirector.time = _time;
                }
                else
                {
                    timelineController.BreakingLoop -= Break;
                    timelineController.PlayableDirector.extrapolationMode = _directorWrapMode;
                }
            }
        }
    }
}