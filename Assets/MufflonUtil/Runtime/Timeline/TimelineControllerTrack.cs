using System;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackClipType(typeof(ITimelineControllerPlayableAsset))]
    [TrackBindingType(typeof(TimelineController))]
    [TrackColor(1f, 0.2f, 0.2f)]
    [Serializable]
    public class TimelineControllerTrack : TrackAsset
    { }

    public interface ITimelineControllerPlayableAsset
    { }
}