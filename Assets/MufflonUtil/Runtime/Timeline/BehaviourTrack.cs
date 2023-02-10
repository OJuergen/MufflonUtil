using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackBindingType(typeof(UnityEngine.Behaviour))]
    [TrackColor(.5f, 1f, .5f)]
    [TrackClipType(typeof(IClipType))]
    public class BehaviourTrack : TimelineTrack<Behaviour>
    {
        private interface IClipType
        { }

        public abstract class Clip<T> : ClipPlayableAsset<T>, IClipType where T : ClipBehaviour, new()
        { }
    }
}