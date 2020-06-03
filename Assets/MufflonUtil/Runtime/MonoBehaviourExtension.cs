using UnityEngine;

namespace MufflonUtil
{
    public static class MonoBehaviourExtension
    {
        /// <summary>
        /// Returns the object itself or null, if gameObject is destroyed.
        /// </summary>
        public static T CheckNull<T>(this T mb) where T : MonoBehaviour
        {
            return mb == null ? null : mb;
        }
    }
}
