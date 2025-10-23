using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <summary>
    /// Base class for custom <see cref="TrackAsset"/>s that pass on the reference to the <see cref="TimelineClip"/>
    /// to the associated <see cref="ClipPlayableAsset"/> when creating <see cref="Playable"/>s.
    /// </summary>
    [TrackColor(1f, 1f, 1f)]
    [Serializable]
    public abstract class TimelineTrack : TrackAsset
    {
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            var timelineClipAsset = clip.asset as ClipPlayableAsset;
            if (timelineClipAsset != null) timelineClipAsset.Clip = clip;
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }

    /// <summary>
    /// Base class for custom <see cref="TimelineTrack"/>s with a predefined <see cref="ClipBehaviour"/> class for
    /// <see cref="TimelineBehaviour{T}"/> with the bound <see cref="TComponent"/> type.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component bound to the track.</typeparam>
    public abstract class TimelineTrack<TComponent> : TimelineTrack
        where TComponent : Component
    {
        public abstract class ClipBehaviour : TimelineClipBehaviour<TComponent>
        { }
    }

    /// <summary>
    /// Base class for <see cref="TimelineTrack{T}"/>s with a custom mixer behaviour of their own. 
    /// </summary>
    /// <typeparam name="TComponent">The type of the component bound to the track.</typeparam>
    /// <typeparam name="TTrackBehaviour">The type of the mixer behaviour of the track.</typeparam>
    public abstract class TimelineTrack<TComponent, TTrackBehaviour> : TimelineTrack<TComponent>
        where TComponent : Component
        where TTrackBehaviour : TrackBehaviour<TComponent>, new()
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var scriptPlayable = ScriptPlayable<TTrackBehaviour>.Create(graph, inputCount);
            scriptPlayable.GetBehaviour().PlayableAsset = this;
            return scriptPlayable;
        }
    }
}