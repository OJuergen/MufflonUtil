using UnityEngine;

namespace MufflonUtil
{
    public class FixRotation : MonoBehaviour
    {
        [SerializeField] private bool _fixX = true;
        [SerializeField] private bool _fixY;
        [SerializeField] private bool _fixZ = true;
        [SerializeField] private Space _space = Space.World;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void LateUpdate()
        {
            if (_space == Space.World)
            {
                Quaternion rotation = _transform.rotation;
                _transform.rotation = Quaternion.Euler(
                    _fixX ? 0 : rotation.eulerAngles.x,
                    _fixY ? 0 : rotation.eulerAngles.y,
                    _fixZ ? 0 : rotation.eulerAngles.z);
            }

            if (_space == Space.Self)
            {
                Quaternion localRotation = _transform.localRotation;
                _transform.localRotation = Quaternion.Euler(
                    _fixX ? 0 : localRotation.eulerAngles.x,
                    _fixY ? 0 : localRotation.eulerAngles.y,
                    _fixZ ? 0 : localRotation.eulerAngles.z);
            }
        }
    }
}