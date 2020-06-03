using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <summary>
    /// Utility wrapper for <see cref="PlayableBehaviour"/> with player data of type <typeparamref name="T"/>.
    /// Must be attached to a <see cref="TrackAsset"/> with a <see cref="TrackBindingTypeAttribute"/>
    /// with type <typeparamref name="T"/>. 
    /// </summary>
    /// <typeparam name="T">The type of the player data</typeparam>
    public class PlayableBehaviour<T> : PlayableBehaviour where T : Component
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
                OnBehaviourStart(Context);
                _initialized = true;
            }

            OnBehaviourUpdate(playable, info, Context);
        }

        /// <summary>
        /// Called once
        /// </summary>
        protected virtual void OnBehaviourStart([NotNull] T playerData)
        { }

        protected virtual void OnBehaviourUpdate(Playable playable, FrameData info, [NotNull] T playerData)
        { }

        public sealed override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (_initialized)
            {
                _initialized = false;
                if (Context != null) OnBehaviourStop(playable, info, Context);
            }
        }

        protected virtual void OnBehaviourStop(Playable playable, FrameData info, [NotNull] T playerData)
        { }
    }
}