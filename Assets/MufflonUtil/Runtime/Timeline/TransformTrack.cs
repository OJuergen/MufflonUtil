using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackBindingType(typeof(Transform))]
    [TrackColor(.75f, .75f, 0f)]
    [TrackClipType(typeof(IClip))]
    public class TransformTrack : TimelineTrack<Transform, TransformTrack.TransformMixerBehaviour>
    {
        /// <summary>
        /// Base class of the <see cref="Behaviour"/> of a <see cref="Clip{T}"/> on a <see cref="TransformTrack"/>. 
        /// </summary>
        public class TransformBehaviour : Behaviour
        {
            [field: SerializeField] public bool AffectsPosition { get; private set; }
            [field: SerializeField] public bool AffectsRotation { get; private set; }
            [field: SerializeField] public bool AffectsScale { get; private set; }
            public Vector3 Position { get; protected set; }
            public Quaternion Rotation { get; protected set; }
            public Vector3 Scale { get; protected set; }
        }

        /// <summary>
        /// Annotation Interface for <see cref="PlayableAsset"/>s that can be added to a <see cref="TransformTrack"/>. 
        /// </summary>
        private interface IClip
        { }

        /// <summary>
        /// A <see cref="ClipPlayableAsset{T}"/> with a <see cref="TransformBehaviour"/>.
        /// Can be added to a <see cref="TransformTrack"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class Clip<T> : ClipPlayableAsset<T>, IClip where T : TransformBehaviour, new()
        { }

        /// <summary>
        /// Mixer behaviour of a <see cref="TransformTrack"/>.
        /// Computes a weighted average over all input clips and controls the bound <see cref="Transform"/> accordingly.
        /// </summary>
        [Serializable]
        public class TransformMixerBehaviour : MixerBehaviour<Transform>
        {
            [SerializeField, Tooltip("If checked, updates to transform will be smoothed by a lerp")]
            private bool _doLerp;

            [SerializeField, ConditionallyVisible(nameof(_doLerp)), Tooltip("Lerp amount per second.")]
            private float _lerpFactor;

            [SerializeField, Tooltip("If checked, behaviour will be previewed in edit mode.")]
            private bool _executeInEditMode;

            [SerializeField, Tooltip("If checked, transform values are cached and reset when preview is stopped")]
            private bool _resetInEditMode = true;

            [SerializeField, Tooltip("If checked, transform values are cached and reset when playback has finished")]
            private bool _reset;

            // cached values for resetting
            private Vector3 _startPosition;
            private Quaternion _startRotation;
            private Vector3 _startScale;

            protected override void OnStart(Transform transform)
            {
                // cache values for reset
                _startPosition = transform.position;
                _startRotation = transform.rotation;
                _startScale = transform.localScale;
            }

            protected override void OnStop(Playable playable, FrameData info, Transform transform)
            {
                // reset cached values
                if (_reset || _resetInEditMode && !Application.isPlaying)
                {
                    transform.position = _startPosition;
                    transform.rotation = _startRotation;
                    transform.localScale = _startScale;
                }
            }

            protected override void OnUpdate(Playable playable, FrameData info, Transform transform)
            {
                if (!Application.isPlaying && !_executeInEditMode) return;

                // determine setpoint values as weighted average over all inputs
                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                Vector3 scale = Vector3.one;
                float positionWeight = 0;
                float rotationWeight = 0;
                float scaleWeight = 0;
                for (var i = 0; i < playable.GetInputCount(); i++)
                {
                    float weight = playable.GetInputWeight(i);
                    if (weight == 0) continue;
                    var input = (ScriptPlayable<TransformBehaviour>)playable.GetInput(i);
                    TransformBehaviour transformBehaviour = input.GetBehaviour();

                    if (transformBehaviour.AffectsPosition)
                    {
                        positionWeight += weight;
                        position = Vector3.Lerp(position, transformBehaviour.Position, weight / positionWeight);
                    }

                    if (transformBehaviour.AffectsRotation)
                    {
                        rotationWeight += weight;
                        rotation = Quaternion.Slerp(rotation, transformBehaviour.Rotation, weight / rotationWeight);
                    }

                    if (transformBehaviour.AffectsScale)
                    {
                        scaleWeight += weight;
                        scale = Vector3.Lerp(scale, transformBehaviour.Scale, weight / scaleWeight);
                    }
                }

                position = Vector3.Lerp(transform.position, position, positionWeight);
                rotation = Quaternion.Slerp(transform.rotation, rotation, rotationWeight);
                scale = Vector3.Lerp(transform.localScale, scale, scaleWeight);

                float lerpValue = _doLerp ? _lerpFactor * Time.deltaTime : 1;
                transform.position = Vector3.Lerp(transform.position, position, lerpValue);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lerpValue);
                transform.localScale = Vector3.Lerp(transform.localScale, scale, lerpValue);
            }
        }
    }
}