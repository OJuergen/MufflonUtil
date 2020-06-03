using UnityEngine;

namespace MufflonUtil
{
    public class FaceCamera : MonoBehaviour
    {
        [SerializeField] private bool _fixXRotation;
        [SerializeField] private bool _fixYRotation;
        [SerializeField] private bool _fixZRotation;

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
            );
        }
    }
}