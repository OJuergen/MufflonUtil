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
        [SerializeField] private bool _faceAwayFromCamera;
        private Transform _mainCamTransform;

        protected override void OnBehaviourStart(Transform transform)
        {
            Camera mainCam = Camera.main;
            _mainCamTransform = mainCam != null ? mainCam.transform : null;
        }

        protected override void OnBehaviourUpdate(Playable playable, FrameData info, Transform transform)
        {
            if (!Application.isPlaying && !_executeInEditMode || _mainCamTransform == null) return;
            Vector3 cameraPosition = _mainCamTransform.position;

            Vector3 currentPosition = transform.position;
            Vector3 setpointPosition = cameraPosition + _mainCamTransform.TransformDirection(_relativeSetpointPosition);
            transform.position = Vector3.Lerp(currentPosition, setpointPosition, _lerpFactor * Time.deltaTime);

            Quaternion lookRotation = _faceAwayFromCamera 
                ? Quaternion.LookRotation(currentPosition - cameraPosition)
                : Quaternion.LookRotation(cameraPosition - currentPosition);
            Quaternion setpointRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(currentRotation, setpointRotation, _lerpFactor * Time.deltaTime);
        }
    }
}