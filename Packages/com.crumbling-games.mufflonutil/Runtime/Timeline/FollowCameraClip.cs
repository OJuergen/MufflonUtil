using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [Serializable]
    internal class FollowCameraClip : TransformTrack.Clip<FollowCameraClip.ClipBehaviour>
    {
        [SerializeField] private ClipBehaviour _behaviour;
        [SerializeField] private bool _affectsPosition;
        [SerializeField] private bool _affectsRotation;
        public override bool AffectsPosition => _affectsPosition;
        public override bool AffectsRotation => _affectsRotation;
        public override bool AffectsScale => false;
        protected override ClipBehaviour BehaviourTemplate => _behaviour;
        
        [Serializable]
        internal class ClipBehaviour : TransformTrack.TransformClipBehaviour
        {
            [SerializeField] private Vector3 _relativeSetpointPosition = Vector3.zero;
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

                Vector3 cameraPosition = _mainCamTransform.position;
                Position = cameraPosition + _mainCamTransform.TransformDirection(_relativeSetpointPosition);
                Rotation = _relativeSetpointRotation * Quaternion.LookRotation(cameraPosition - transform.position);
            }
        }
    }
}