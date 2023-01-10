using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackBindingType(typeof(Behaviour))]
    [TrackColor(.5f, 1f, .5f)]
    [TrackClipType(typeof(IClipType))]
    public class BehaviourTrack : TimelineTrack<Behaviour>
    {
        private interface IClipType
        { }

        public abstract class BehaviourClipAsset<T> : ClipAsset<T>, IClipType where T : ClipBehaviour, new()
        { }
    }
}