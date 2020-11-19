using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackClipType(typeof(ITransformPlayableAsset))]
    [TrackBindingType(typeof(Transform))]
    [TrackColor(.75f, .75f, 0f)]
    [Serializable]
    public class TransformTrack : TrackAsset
    { }

    public interface ITransformPlayableAsset
    { }
}