using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [Serializable]
    internal class FollowCameraClip : TransformTrack.Clip<FollowCameraClip.Behaviour>
    {
        [field:SerializeField] protected override Behaviour Template { get; set; }
        
        [Serializable]
        internal class Behaviour : TransformTrack.TransformBehaviour
        {
            [SerializeField] private Vector3 _relativeSetpointPosition = new(0, -0.1f, 0.5f);
            [SerializeField] private Quaternion _relativeSetpointRotation = Quaternion.identity;
            private Transform _mainCamTransform;

            protected override void OnStart(Transform transform)
            {
                Camera mainCam = Camera.main;
                _mainCamTransform = mainCam != null ? mainCam.transform : null;
            }

            protected override void OnUpdate(Playable playable, FrameData info, Transform transform)
            {
                if (_mainCamTransform == null) return;
                Position = _mainCamTransform.position + _mainCamTransform.TransformDirection(_relativeSetpointPosition);

                Vector3 currentPosition = transform.position;
                Vector3 cameraPosition = _mainCamTransform.position;

                Rotation = _relativeSetpointRotation * Quaternion.LookRotation(cameraPosition - currentPosition);
                Scale = transform.localScale;
            }

        }
    }

}