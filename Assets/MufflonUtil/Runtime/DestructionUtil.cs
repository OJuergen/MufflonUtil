using JetBrains.Annotations;
using UnityEngine;

namespace MufflonUtil
{
    [CreateAssetMenu(menuName = "MufflonUtil/Destruction Util", fileName = "DestructionUtil")]
    public class DestructionUtil : ScriptableObject
    {
        [UsedImplicitly]
        public void Destroy(GameObject gameObject)
        {
            if (gameObject != null) Object.Destroy(gameObject);
        }
    }
}