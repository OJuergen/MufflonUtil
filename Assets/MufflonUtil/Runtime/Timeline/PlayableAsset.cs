using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    /// <summary>
    /// Utility wrapper for <see cref="PlayableAsset"/> that creates a <see cref="ScriptPlayable{T}"/>
    /// from a template. Unfortunately, using this inherited template will not allow for animating its values.
    /// If you need to animate the values, you need to have the template field on the inherited class.
    /// </summary>
    public abstract class PlayableAsset<TComponent, TBehaviour> : PlayableAsset
        where TBehaviour : PlayableBehaviour<TComponent>, new()
        where TComponent : Component
    {
        [SerializeField] private TBehaviour _template;
        protected TBehaviour Template => _template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<TBehaviour>.Create(graph, _template);
        }
    }
}