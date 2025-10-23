using JetBrains.Annotations;
using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Utility asset to allow destroying objects with a non-static method that can be used from Unity Events.
    /// </summary>
    [CreateAssetMenu(menuName = "MufflonUtil/Destruction Util", fileName = "DestructionUtil")]
    public class DestructionUtil : ScriptableObject
    {
        [UsedImplicitly]
        public new void Destroy(Object objectToDestroy)
        {
            if (objectToDestroy != null) Object.Destroy(objectToDestroy);
        }
    }
}