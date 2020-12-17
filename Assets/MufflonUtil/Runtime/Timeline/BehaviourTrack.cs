using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackClipType(typeof(IPlayableAsset))]
    [TrackBindingType(typeof(UnityEngine.Behaviour))]
    [TrackColor(.5f, 1f, .5f)]
    public class BehaviourTrack : TrackAsset
    {
        private interface IPlayableAsset
        { }

        public class PlayableAsset<T> : PlayableAsset<UnityEngine.Behaviour, T>, IPlayableAsset where T : Behaviour, new()
        { }

        public class Behaviour : PlayableBehaviour<UnityEngine.Behaviour>
        { }
    }
}