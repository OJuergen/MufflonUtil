using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackColor(1f, .5f, 0f)]
    [TrackBindingType(typeof(Renderer))]
    [TrackClipType(typeof(IClipType))]
    public class RendererTrack : TimelineTrack<Renderer>
    {
        private interface IClipType
        { }

        public abstract class RendererClipAsset<T> : ClipAsset<T>, IClipType where T : ClipBehaviour, new()
        { }
    }
}