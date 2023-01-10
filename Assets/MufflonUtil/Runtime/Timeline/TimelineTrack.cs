using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <summary>
    /// Base class for custom timeline tracks.
    /// </summary>
    [TrackColor(1f, 1f, 1f)]
    [Serializable]
    public abstract class TimelineTrack : TrackAsset
    {
        public abstract class ClipAsset : PlayableAsset
        {
            public TimelineClip Clip { get; set; }
        }
    }

    public abstract class
        TimelineTrack<TComponent> : TimelineTrack<TComponent, TimelineTrack<TComponent>.MixerBehaviour>
        where TComponent : Component
    {
        public class MixerBehaviour : TimelineClipBehaviour<TComponent>
        {
            protected override void OnUpdate(Playable playable, FrameData info, TComponent playerData)
            {
                for (var i = 0; i < playable.GetInputCount(); i++)
                {
                    var input = (ScriptPlayable<ClipBehaviour>)playable.GetInput(i);
                    ClipBehaviour clipBehaviour = input.GetBehaviour();
                    clipBehaviour.ProcessFrame(playable, info, playerData);
                }
            }
        }
    }

    public abstract class TimelineTrack<TComponent, TMixerBehaviour> : TimelineTrack
        where TComponent : Component
        where TMixerBehaviour : TimelineTrack<TComponent>.MixerBehaviour, new()
    {
        [SerializeField] private TMixerBehaviour _mixer;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TMixerBehaviour>.Create(graph, _mixer, inputCount);
        }

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            var timelineClipAsset = clip.asset as ClipAsset;
            if (timelineClipAsset != null) timelineClipAsset.Clip = clip;
            return base.CreatePlayable(graph, gameObject, clip);
        }

        public class ClipBehaviour : TimelineClipBehaviour<TComponent>
        { }

        /// <summary>
        /// Utility wrapper for <see cref="PlayableAsset"/> that creates a <see cref="ScriptPlayable{T}"/>
        /// from a template.
        /// In order to allow animating the values of the <see cref="Template"/>, the inheriting class needs to
        /// provide a serialized field of type <see cref="TBehaviour"/> and have <see cref="Template"/> return this.
        /// </summary>
        public abstract class ClipAsset<TBehaviour> : ClipAsset where TBehaviour : ClipBehaviour, new()
        {
            /// <summary>
            /// Override this with a serialized backing field to configure properties in the editor.
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
}