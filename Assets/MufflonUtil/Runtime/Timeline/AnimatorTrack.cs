using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackBindingType(typeof(Animator))]
    [TrackColor(0.25f, 1f, 0.75f)]
    [TrackClipType(typeof(IClipType))]
    public class AnimatorTrack : TimelineTrack<Animator>
    {
        public class AnimatorClipBehaviour : ClipBehaviour
        {
            protected Animator Animator => Context;
        }

        private interface IClipType
        { }

        public abstract class AnimatorClipAsset<T> : ClipAsset<T>, IClipType where T : AnimatorClipBehaviour, new()
        { }
    }
}