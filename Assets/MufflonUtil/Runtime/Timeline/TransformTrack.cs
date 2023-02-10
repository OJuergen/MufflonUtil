using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    [TrackBindingType(typeof(Transform))]
    [TrackColor(.75f, .75f, 0f)]
    [TrackClipType(typeof(IClip))]
    public class TransformTrack : TimelineTrack<Transform, TransformTrack.TransformTrackBehaviour>
    {
        [field: SerializeField, Tooltip("If checked, updates to transform will be smoothed by a lerp")]
        private bool DoLerp { get; set; } = true;

        [field: SerializeField, Tooltip("Lerp amount per second.")]
        private float LerpFactor { get; set; } = 3;

        [field: SerializeField, Tooltip("If checked, behaviour will be previewed in edit mode.")]
        private bool ExecuteInEditMode { get; set; } = true;

        [field: SerializeField, Tooltip("If checked, transform values are cached and reset when preview is stopped")]
        private bool ResetInEditMode { get; set; } = true;

        [field: SerializeField, Tooltip("If checked, transform values are cached and reset when playback has finished")]
        private bool Reset { get; set; }
        
        /// <summary>
        /// Base class of the <see cref="ClipBehaviour"/> of a <see cref="Clip{T}"/> on a <see cref="TransformTrack"/>. 
        /// </summary>
        public class TransformClipBehaviour : ClipBehaviour
        {
            private IClip TransformClip => ClipAsset as IClip;
            public bool AffectsPosition => TransformClip.AffectsPosition;
            public bool AffectsRotation => TransformClip.AffectsRotation;
            public bool AffectsScale => TransformClip.AffectsScale;
            public Vector3 Position { get; protected set; }
            public Quaternion Rotation { get; protected set; }
            public Vector3 Scale { get; protected set; }
        }

        /// <summary>
        /// Annotation Interface for <see cref="PlayableAsset"/>s that can be added to a <see cref="TransformTrack"/>. 
        /// </summary>
        private interface IClip
        {
            bool AffectsPosition { get; }
            bool AffectsRotation { get; }
            bool AffectsScale { get; }
        }

        /// <summary>
        /// A <see cref="ClipPlayableAsset{T}"/> with a <see cref="TransformClipBehaviour"/>.
        /// Can be added to a <see cref="TransformTrack"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class Clip<T> : ClipPlayableAsset<T>, IClip where T : TransformClipBehaviour, new()
        {
            public abstract bool AffectsPosition { get; }
            public abstract bool AffectsRotation { get; }
            public abstract bool AffectsScale { get; }
        }

        /// <summary>
        /// Mixer behaviour of a <see cref="TransformTrack"/>.
        /// Computes a weighted average over all input clips and controls the bound <see cref="Transform"/> accordingly.
        /// </summary>
        [Serializable]
        public class TransformTrackBehaviour : TrackBehaviour<Transform>
        {
            TransformTrack TransformTrack => TrackAsset as TransformTrack;
            
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
                if (TransformTrack.Reset || TransformTrack.ResetInEditMode && !Application.isPlaying)
                {
                    transform.position = _startPosition;
                    transform.rotation = _startRotation;
                    transform.localScale = _startScale;
                }
            }

            protected override void OnUpdate(Playable playable, FrameData info, Transform transform)
            {
                if (!Application.isPlaying && !TransformTrack.ExecuteInEditMode) return;

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
                    var input = (ScriptPlayable<TransformClipBehaviour>)playable.GetInput(i);
                    TransformClipBehaviour transformClipBehaviour = input.GetBehaviour();

                    if (transformClipBehaviour.AffectsPosition)
                    {
                        positionWeight += weight;
                        position = Vector3.Lerp(position, transformClipBehaviour.Position, weight / positionWeight);
                    }

                    if (transformClipBehaviour.AffectsRotation)
                    {
                        rotationWeight += weight;
                        rotation = Quaternion.Slerp(rotation, transformClipBehaviour.Rotation, weight / rotationWeight);
                    }

                    if (transformClipBehaviour.AffectsScale)
                    {
                        scaleWeight += weight;
                        scale = Vector3.Lerp(scale, transformClipBehaviour.Scale, weight / scaleWeight);
                    }
                }

                position = Vector3.Lerp(transform.position, position, positionWeight);
                rotation = Quaternion.Slerp(transform.rotation, rotation, rotationWeight);
                scale = Vector3.Lerp(transform.localScale, scale, scaleWeight);

                float lerpValue = TransformTrack.DoLerp ? TransformTrack.LerpFactor * Time.deltaTime : 1;
                transform.position = Vector3.Lerp(transform.position, position, lerpValue);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lerpValue);
                transform.localScale = Vector3.Lerp(transform.localScale, scale, lerpValue);
            }
        }
    }
}