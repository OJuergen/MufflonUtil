using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackColor(1f, .5f, 0f)]
    [TrackBindingType(typeof(PlayableDirector))]
    [TrackClipType(typeof(IClipType))]
    public class PlayableDirectorTrack : TimelineTrack<PlayableDirector>
    {
        private interface IClipType
        { }

        public abstract class PlayableDirectorClipAsset<T> : ClipAsset<T>, IClipType where T : ClipBehaviour, new()
        { }
    }
}