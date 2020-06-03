using UnityEngine;

namespace MufflonUtil
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;
        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }
    }
}
