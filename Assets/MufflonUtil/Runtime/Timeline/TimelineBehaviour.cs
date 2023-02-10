using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <summary>
    /// base class of a <see cref="PlayableBehaviour"/> with a reference to a <see cref="TimelineClip"/>.
    /// </summary>
    public abstract class TimelineBehaviour : PlayableBehaviour
    {
        public PlayableAsset PlayableAsset { get; set; }
    }

    public abstract class TimelineClipBehaviour<T> : TimelineBehaviour<T> where T : Component
    {
        public ClipPlayableAsset ClipAsset => PlayableAsset as ClipPlayableAsset;
    }

    /// <summary>
    /// Base class of a <see cref="TimelineBehaviour"/> with player data of type <typeparamref name="T"/>.
    /// Must be attached to a <see cref="TrackAsset"/> with a <see cref="TrackBindingTypeAttribute"/>
    /// of type <typeparamref name="T"/>. 
    /// </summary>
    /// <typeparam name="T">The type of the player data bound to the track.</typeparam>
    public abstract class TimelineBehaviour<T> : TimelineBehaviour where T : Component
    {
        /// <summary>
        /// The object bound to the track. Available after the first frame was processed.
        /// </summary>
        protected T Context { get; private set; }
        private bool _initialized;
        public sealed override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Context = playerData as T;
            if (Context == null)
            {
                Debug.LogWarning($"No Component of type {typeof(T)} assigned to track.");
                return;
            }
            if (!_initialized)
            {
                OnStart(Context);
                _initialized = true;
            }

            OnUpdate(playable, info, Context);
        }

        /// <summary>
        /// Called once
        /// </summary>
        protected virtual void OnStart([NotNull] T playerData)
        { }

        protected virtual void OnUpdate(Playable playable, FrameData info, [NotNull] T playerData)
        { }

        public sealed override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (_initialized)
            {
                _initialized = false;
                if (Context != null) OnStop(playable, info, Context);
            }
        }

        protected virtual void OnStop(Playable playable, FrameData info, [NotNull] T playerData)
        { }
    }
}