using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackClipType(typeof(IPlayableAsset))]
    [TrackBindingType(typeof(Renderer))]
    [TrackColor(1f, .5f, 0f)]
    public class RendererTrack : TrackAsset
    {
        private interface IPlayableAsset
        { }

        public class PlayableAsset<T> : PlayableAsset<Renderer, T>, IPlayableAsset where T : Behaviour, new()
        { }

        public class Behaviour : PlayableBehaviour<Renderer>
        { }
    }
}