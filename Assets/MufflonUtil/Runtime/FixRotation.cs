using UnityEngine;

namespace MufflonUtil
{
    public class FixRotation : MonoBehaviour
    {
        [SerializeField] private bool _fixX = true;
        [SerializeField] private bool _fixY;
        [SerializeField] private bool _fixZ = true;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void LateUpdate()
        {
            Quaternion rotation = _transform.rotation;
            _transform.rotation = Quaternion.Euler(
                _fixX ? 0 : rotation.eulerAngles.x,
                _fixY ? 0 : rotation.eulerAngles.y,
                _fixZ ? 0 : rotation.eulerAngles.z);
        }
    }
}