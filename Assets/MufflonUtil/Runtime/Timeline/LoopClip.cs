using System.ComponentModel;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [DisplayName("Timeline Control/Loop")]
    public class LoopClip : TimelineControllerTrack.Clip<LoopClip.Behaviour>
    {
        protected override Behaviour Template { get; set; }

        public class Behaviour : TimelineControllerTrack.Behaviour
        {
            private bool _isInitialized;
            private bool _broken;
            private DirectorWrapMode _directorWrapMode;
            private PlayableAsset _playableAsset;

            private void Break()
            {
                _broken = true;
                Context.PlayableDirector.extrapolationMode = _directorWrapMode;
            }

            protected override void OnStart(TimelineController timelineController)
            {
                _broken = false;
                _isInitialized = false;
                PlayableDirector playableDirector = timelineController.PlayableDirector;
                _playableAsset = playableDirector.playableAsset;
                _directorWrapMode = playableDirector.extrapolationMode;
                // prevent director from stopping when timeline ends with loop
                playableDirector.extrapolationMode = DirectorWrapMode.Hold;
            }

            protected override void OnUpdate(Playable playable, FrameData info, TimelineController timelineController)
            {
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    timelineController.BreakingLoop += Break;
                }

                bool isOutOfClipNextFrame = timelineController.PlayableDirector.time + info.deltaTime > PlayableAsset.Clip.end;
                if (!_broken && isOutOfClipNextFrame) timelineController.PlayableDirector.time = PlayableAsset.Clip.start;
            }

            protected override void OnStop(Playable playable, FrameData info,
                TimelineController timelineController)
            {
                PlayableDirector playableDirector = timelineController.PlayableDirector;
                if (playableDirector == null) return;
                if (!_broken && playableDirector.state == PlayState.Playing &&
                    playableDirector.playableAsset == _playableAsset)
                {
                    playableDirector.time = PlayableAsset.Clip.start;
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