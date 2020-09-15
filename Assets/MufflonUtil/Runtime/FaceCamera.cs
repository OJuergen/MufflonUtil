using UnityEngine;

namespace MufflonUtil
{
    public class FaceCamera : MonoBehaviour
    {
        [SerializeField, Tooltip("If true, euler angle X will be set to zero")]
        private bool _fixXRotation;
        [SerializeField, Tooltip("If true, euler angle Y will be set to zero")]
        private bool _fixYRotation;
        [SerializeField, Tooltip("If true, euler angle Z will be set to zero")]
        private bool _fixZRotation;
        [SerializeField, Tooltip("Additional rotation applied to the object")]
        private Vector3 _offset;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            enabled = _camera != null;
        }

        private void LateUpdate()
        {
            if (!enabled) return; // in tests, LateUpdate gets called at least once even when script is disabled
            Quaternion lookRotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
            transform.rotation = Quaternion.Euler(
                _fixXRotation ? 0 : lookRotation.eulerAngles.x,
                _fixYRotation ? 0 : lookRotation.eulerAngles.y,
                _fixZRotation ? 0 : lookRotation.eulerAngles.z
            ) * Quaternion.Euler(_offset);
        }
    }
}