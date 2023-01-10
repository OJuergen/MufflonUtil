using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackBindingType(typeof(Transform))]
    [TrackColor(.75f, .75f, 0f)]
    [TrackClipType(typeof(IClipType))]
    public class TransformTrack : TimelineTrack<Transform, TransformTrack.TransformMixerBehaviour>
    {
        public class TransformClipBehaviour : ClipBehaviour
        {
            public Vector3 Position { get; protected set; }
            public Quaternion Rotation { get; protected set; }
            public Vector3 Scale { get; protected set; }
        }

        private interface IClipType
        { }

        public abstract class TransformClipAsset<T> : TimelineTrack<Transform, TransformMixerBehaviour>.ClipAsset<T>,
            IClipType where T : TransformClipBehaviour, new()
        { }

        /// <summary>
        /// Sealed. Inherit from <see cref="TransformClipAsset{T}"/> instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public new sealed class ClipAsset<T> : TimelineTrack<Transform, TransformMixerBehaviour>.ClipAsset<T>
            where T : ClipBehaviour, new()
        {
            protected override T Template { get; set; }
        }

        [Serializable]
        public class TransformMixerBehaviour : TimelineTrack<Transform>.MixerBehaviour
        {
            [SerializeField] private float _lerpFactor;
            [SerializeField] private bool _executeInEditMode;
            private Vector3 _startPosition;
            private Quaternion _startRotation;
            [SerializeField] private bool _resetInEditMode = true;
            [SerializeField] private bool _reset;

            protected override void OnStart(Transform transform)
            {
                _startPosition = transform.position;
                _startRotation = transform.rotation;
            }

            protected override void OnStop(Playable playable, FrameData info, Transform transform)
            {
                if (_reset || _resetInEditMode && !Application.isPlaying)
                {
                    transform.position = _startPosition;
                    transform.rotation = _startRotation;
                }
            }

            protected override void OnUpdate(Playable playable, FrameData info, Transform transform)
            {
                if (!Application.isPlaying && !_executeInEditMode) return;

                float totalWeight = 0;
                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                Vector3 scale = Vector3.one;

                for (var i = 0; i < playable.GetInputCount(); i++)
                {
                    var input = (ScriptPlayable<TransformClipBehaviour>)playable.GetInput(i);
                    TransformClipBehaviour timelineClipBehaviour = input.GetBehaviour();
                    float weight = playable.GetInputWeight(i);
                    totalWeight += weight;
                    if (totalWeight == 0) continue;
                    position = Vector3.Lerp(position, timelineClipBehaviour.Position, weight / totalWeight);
                    rotation = Quaternion.Slerp(rotation, timelineClipBehaviour.Rotation, weight / totalWeight);
                    scale = Vector3.Lerp(scale, timelineClipBehaviour.Scale, weight / totalWeight);
                }

                position = Vector3.Lerp(transform.position, position, totalWeight);
                rotation = Quaternion.Slerp(transform.rotation, rotation, totalWeight);
                scale = Vector3.Lerp(transform.localScale, scale, totalWeight);

                transform.position = Vector3.Lerp(transform.position, position, _lerpFactor * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _lerpFactor * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, scale, _lerpFactor * Time.deltaTime);
            }
        }
    }
}