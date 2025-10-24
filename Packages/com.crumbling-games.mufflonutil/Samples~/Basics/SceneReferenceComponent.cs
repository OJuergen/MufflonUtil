using UnityEngine;

namespace MufflonUtil.Samples.Basics
{
    /// <summary>
    /// A component to demonstrate <see cref="SceneReference"/>, referencing scene assets to load them independently
    /// of their name, path and build index.
    /// </summary>
    public class SceneReferenceComponent : MonoBehaviour
    {
        [SerializeField] private SceneReference _scene;
    }
}