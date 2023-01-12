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
    /// from a <see cref="Template"/> and passes on its <see cref="TimelineClip"/> reference.
    /// <br/>
    /// In order to animate the values of the <see cref="Template"/>, the inheriting class needs to
    /// provide a serialized backing field of type <see cref="TBehaviour"/> for <see cref="Template"/>.
    /// </summary>
    public abstract class ClipPlayableAsset<TBehaviour> : ClipPlayableAsset where TBehaviour : TimelineBehaviour, new()
    {
        /// <summary>
        /// Override this with a serialized backing field to configure and animate properties.
        /// </summary>
        protected abstract TBehaviour Template { get; set; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            if (Template == null) Template = new TBehaviour();
            Template.Clip = Clip;
            return ScriptPlayable<TBehaviour>.Create(graph, Template);
        }
    }
}