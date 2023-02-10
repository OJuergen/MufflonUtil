using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public class AnimatorPauseClip : AnimatorTrack.Clip<AnimatorPauseClip.ClipBehaviour>
    {
        [field: SerializeField]
        private PostPlaybackBehaviour PostPlaybackBehaviour { get; set; } = PostPlaybackBehaviour.Revert;

        public class ClipBehaviour : AnimatorTrack.AnimatorClipBehaviour
        {
            private AnimatorPauseClip PauseClip => ClipAsset as AnimatorPauseClip;
            private bool _previousValue;

            protected override void OnStart(Animator animator)
            {
                _previousValue = Animator.enabled;
                Animator.enabled = false;
            }

            protected override void OnStop(Playable playable, FrameData info, Animator animator)
            {
                switch (PauseClip.PostPlaybackBehaviour)
                {
                    case PostPlaybackBehaviour.Active:
                        animator.enabled = true;
                        break;
                    case PostPlaybackBehaviour.Revert:
                        animator.enabled = _previousValue;
                        break;
                    case PostPlaybackBehaviour.Inactive:
                        animator.enabled = false;
                        break;
                    case PostPlaybackBehaviour.KeepAsIs:
                        break;
                }
            }
        }
    }
}