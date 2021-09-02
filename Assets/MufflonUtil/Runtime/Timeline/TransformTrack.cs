using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackClipType(typeof(ITransformPlayableAsset))]
    [TrackBindingType(typeof(Transform))]
    [TrackColor(.75f, .75f, 0f)]
    [Serializable]
    public class TransformTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TransformMixerBehaviour>.Create(graph, inputCount);
        }
    }

    public interface ITransformPlayableAsset
    { }

    public class TransformMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            for (var i = 0; i < playable.GetInputCount(); i++)
            {
                var input = (ScriptPlayable<PlayableBehaviour<Transform>>) playable.GetInput(i);
                PlayableBehaviour<Transform> playableBehaviour = input.GetBehaviour();
                if (playableBehaviour is WeightedPlayableBehaviour<Transform> tb)
                    tb.Weight = playable.GetInputWeight(i);
            }
        }
    }
}