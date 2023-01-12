using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <summary>
    /// A <see cref="TimelineTrack{T}"/> with a <see cref="TimelineController"/> bound to it.
    /// Can be used to manipulate the way the <see cref="TimelineAsset"/> is played through, e.g., loop and jump clips. 
    /// </summary>
    [TrackBindingType(typeof(TimelineController))]
    [TrackColor(1f, 0.2f, 0.2f)]
    [TrackClipType(typeof(IClip))]
    public class TimelineControllerTrack : TimelineTrack<TimelineController>
    {
        private interface IClip
        { }

        public abstract class Clip<T> : ClipPlayableAsset<T>, IClip where T : Behaviour, new()
        { }
    }
}