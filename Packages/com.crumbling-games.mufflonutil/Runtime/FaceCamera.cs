using UnityEngine;

namespace MufflonUtil
{
    [ExecuteAlways]
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

        private void LateUpdate()
        {
            if (!enabled) return; // in tests, LateUpdate gets called at least once even when script is disabled
            Camera cam = Camera.main;
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                cam = UnityEditor.SceneView.lastActiveSceneView?.camera;
            }
#endif

            if (cam == null) return;
            Quaternion lookRotation = Quaternion.LookRotation(transform.position - cam.transform.position);
            transform.rotation = Quaternion.Euler(
                _fixXRotation ? 0 : lookRotation.eulerAngles.x,
                _fixYRotation ? 0 : lookRotation.eulerAngles.y,
                _fixZRotation ? 0 : lookRotation.eulerAngles.z
            ) * Quaternion.Euler(_offset);
        }
    }
}