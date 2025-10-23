using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// A <see cref="TimelineBehaviour{T}"/> for a <see cref="TimelineTrack{T, U}"/>.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class TrackBehaviour<TComponent> : TimelineBehaviour<TComponent> where TComponent : Component
    {
        public TimelineTrack<TComponent> TrackAsset => PlayableAsset as TimelineTrack<TComponent>;
    }
}