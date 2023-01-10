using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackBindingType(typeof(TimelineController))]
    [TrackColor(1f, 0.2f, 0.2f)]
    [TrackClipType(typeof(IClipType))]
    public class TimelineControllerTrack : TimelineTrack<TimelineController>
    {
        private interface IClipType
        { }

        public abstract class TimelineControllerClipAsset<T> : ClipAsset<T>, IClipType where T : ClipBehaviour, new()
        { }
    }
}