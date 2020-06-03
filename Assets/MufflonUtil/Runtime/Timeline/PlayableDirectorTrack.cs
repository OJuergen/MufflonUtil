using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackClipType(typeof(IPlayableAsset))]
    [TrackBindingType(typeof(PlayableDirector))]
    [TrackColor(1f, .5f, 0f)]
    public class PlayableDirectorTrack : TrackAsset
    {
        private interface IPlayableAsset
        { }

        public class PlayableAsset<T> : PlayableAsset<PlayableDirector, T>, IPlayableAsset where T : Behaviour, new()
        { }

        public class Behaviour : PlayableBehaviour<PlayableDirector>
        { }
    }
}