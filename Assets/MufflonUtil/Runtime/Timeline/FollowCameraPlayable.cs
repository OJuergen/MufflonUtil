using System;
using UnityEngine;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [Serializable]
    internal class FollowCameraPlayable : PlayableAsset, ITransformPlayableAsset
    {
        [SerializeField] private FollowCameraPlayableBehaviour _template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<FollowCameraPlayableBehaviour>.Create(graph, _template);
        }
    }

    [Serializable]
    internal class FollowCameraPlayableBehaviour : PlayableBehaviour<Transform>
    {
        [SerializeField] private float _lerpFactor = 5;
        [SerializeField] private Vector3 _relativeSetpointPosition = new Vector3(0, -0.1f, 0.5f);
        [SerializeField] private bool _executeInEditMode;
        private Transform _mainCamTransform;

        protected override void OnBehaviourStart(Transform transform)
        {
            var mainCam = Camera.main;
            _mainCamTransform = mainCam != null ? mainCam.transform : null;
        }

        protected override void OnBehaviourUpdate(Playable playable, FrameData info, Transform transform)
        {
            if (!Application.isPlaying && !_executeInEditMode) return;
            var cameraPosition = _mainCamTransform.position;

            var currentPosition = transform.position;
            var setpointPosition = cameraPosition + _mainCamTransform.TransformDirection(_relativeSetpointPosition);
            transform.position = Vector3.Lerp(currentPosition, setpointPosition, _lerpFactor * Time.deltaTime);

            var lookRotation = Quaternion.LookRotation(cameraPosition - currentPosition);
            var setpointRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            var currentRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(currentRotation, setpointRotation, _lerpFactor * Time.deltaTime);
        }
    }
}