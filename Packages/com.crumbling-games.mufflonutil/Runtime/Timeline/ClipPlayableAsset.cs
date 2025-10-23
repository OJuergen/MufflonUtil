using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <summary>
    /// A <see cref="PlayableAsset"/> with a reference to a <see cref="TimelineClip"/>.
    /// </summary>
    public abstract class ClipPlayableAsset : PlayableAsset
    {
        public TimelineClip Clip { get; set; }
    }

    /// <summary>
    /// A <see cref="ClipPlayableAsset"/> that creates a <see cref="ScriptPlayable{T}"/>
    /// from a <see cref="BehaviourTemplate"/> and passes on its <see cref="TimelineClip"/> reference.
    /// <br/>
    /// In order to animate the values of the <see cref="BehaviourTemplate"/>, the inheriting class needs to
    /// provide a serialized backing field of type <see cref="TBehaviour"/> for <see cref="BehaviourTemplate"/>.
    /// </summary>
    public abstract class ClipPlayableAsset<TBehaviour> : ClipPlayableAsset where TBehaviour : TimelineBehaviour, new()
    {
        /// <summary>
        /// Override this with a serialized backing field to configure and animate properties.
        /// </summary>
        // ReSharper disable once UnassignedGetOnlyAutoProperty - must be override for Unity to support inspector in timeline view
        protected virtual TBehaviour BehaviourTemplate { get; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<TBehaviour> scriptPlayable = BehaviourTemplate == null
                ? ScriptPlayable<TBehaviour>.Create(graph)
                : ScriptPlayable<TBehaviour>.Create(graph, BehaviourTemplate);
            scriptPlayable.GetBehaviour().PlayableAsset = this;
            return scriptPlayable;
        }
    }
}