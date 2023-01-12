using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// A <see cref="TimelineBehaviour{T}"/> for a <see cref="TimelineTrack{T, U}"/>.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class MixerBehaviour<TComponent> : TimelineBehaviour<TComponent> where TComponent : Component
    { }
}