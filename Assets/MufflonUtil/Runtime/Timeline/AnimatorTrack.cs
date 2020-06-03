using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackClipType(typeof(IPlayableAsset))]
    [TrackBindingType(typeof(Animator))]
    [TrackColor(0.25f, 1f, 0.75f)]
    public class AnimatorTrack : TrackAsset
    {
        private interface IPlayableAsset
        { }

        public class PlayableAsset<T> : PlayableAsset<Animator, T>, IPlayableAsset where T : PlayableBehaviour, new()
        { }

        public class PlayableBehaviour : PlayableBehaviour<Animator>
        {
            protected Animator Animator => Context;
        }
    }
}