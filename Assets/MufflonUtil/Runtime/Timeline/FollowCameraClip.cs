using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [Serializable]
    internal class FollowCameraClip : TransformTrack.TransformClipAsset<FollowCameraClip.Behaviour>
    {
        [field:SerializeField] protected override Behaviour Template { get; set; }
        
        [Serializable]
        internal class Behaviour : TransformTrack.TransformClipBehaviour
        {
            [SerializeField] private Vector3 _relativeSetpointPosition = new(0, -0.1f, 0.5f);
            [SerializeField] private bool _faceAwayFromCamera;
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

                Quaternion lookRotation = _faceAwayFromCamera 
                    ? Quaternion.LookRotation(currentPosition - cameraPosition)
                    : Quaternion.LookRotation(cameraPosition - currentPosition);
                Rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
                Scale = transform.localScale;
            }

        }
    }

}