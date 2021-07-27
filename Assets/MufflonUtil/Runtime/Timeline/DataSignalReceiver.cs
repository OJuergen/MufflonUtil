using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    public abstract class DataSignalReceiver : MonoBehaviour, INotificationReceiver
    {
        public abstract void OnNotify(Playable origin, INotification notification, object context);
    }
    
    /// <summary>
    /// Listens for emitted data-carrying signals and reacts depending on its defined reactions.
    /// </summary>
    /// A DataSignalReceiver contains a list of reactions. Each reaction is bound to a SignalAsset.
    /// When a DataSignalEmitter emits a signal, the DataSignalReceiver invokes the corresponding reaction and passes on
    /// the data of the emitter.
    /// <seealso cref="DataSignalEmitter{T}"/>
    /// <seealso cref="SignalAsset"/>
    public abstract class DataSignalReceiver<T> : DataSignalReceiver
    {
        [SerializeField] private DataEventKeyValue _events = new DataEventKeyValue();

        /// <summary>
        /// Called when a notification is sent.
        /// </summary>
        public override void OnNotify(Playable origin, INotification notification, object context)
        {
            var signal = notification as DataSignalEmitter<T>;
            if (signal != null && signal.SignalAsset != null)
            {
                if (_events.TryGetValue(signal.SignalAsset, out DataUnityEvent evt) && evt != null)
                {
                    evt.Invoke(signal.Data);
                }
            }
        }

        /// <summary>
        /// Defines a new reaction for a SignalAsset.
        /// </summary>
        /// <param name="asset">The SignalAsset for which the reaction is being defined.</param>
        /// <param name="reaction">The UnityEvent{T} that describes the reaction.</param>
        /// <exception cref="ArgumentNullException">Thrown when the asset is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the SignalAsset is already registered with this receiver.</exception>
        public void AddReaction(SignalAsset asset, DataUnityEvent reaction)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            if (_events.Signals.Contains(asset))
                throw new ArgumentException("SignalAsset already used.");
            _events.Append(asset, reaction);
        }

        /// <summary>
        /// Appends a null SignalAsset with a reaction specified by the UnityEvent.
        /// </summary>
        /// <param name="reaction">The new reaction to be appended.</param>
        /// <returns>The index of the appended reaction.</returns>
        /// <remarks>Multiple null assets are valid.</remarks>
        public int AddEmptyReaction(DataUnityEvent reaction)
        {
            _events.Append(null, reaction);
            return _events.Events.Count - 1;
        }

        /// <summary>
        /// Removes the first occurrence of a SignalAsset.
        /// </summary>
        /// <param name="asset">The SignalAsset to be removed.</param>
        public void Remove(SignalAsset asset)
        {
            if (!_events.Signals.Contains(asset))
            {
                throw new ArgumentException("The SignalAsset is not registered with this receiver.");
            }

            _events.Remove(asset);
        }

        /// <summary>
        /// Gets a list of all registered SignalAssets.
        /// </summary>
        /// <returns>Returns a list of SignalAssets.</returns>
        public IEnumerable<SignalAsset> GetRegisteredSignals()
        {
            return _events.Signals;
        }

        /// <summary>
        /// Gets the first UnityEvent associated with a SignalAsset.
        /// </summary>
        /// <param name="key">A SignalAsset defining the signal.</param>
        /// <returns>Returns the reaction associated with a SignalAsset. Returns null if the signal asset does not exist.</returns>
        public DataUnityEvent GetReaction(SignalAsset key)
        {
            if (_events.TryGetValue(key, out DataUnityEvent ret))
            {
                return ret;
            }

            return null;
        }

        /// <summary>
        /// Returns the count of registered SignalAssets.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _events.Signals.Count;
        }

        /// <summary>
        /// Replaces the SignalAsset associated with a reaction at a specific index.
        /// </summary>
        /// <param name="idx">The index of the reaction.</param>
        /// <param name="newKey">The replacement SignalAsset.</param>
        /// <exception cref="ArgumentException">Thrown when the replacement SignalAsset is already registered to this SignalReceiver.</exception>
        /// <remarks>The new SignalAsset can be null.</remarks>
        public void ChangeSignalAtIndex(int idx, SignalAsset newKey)
        {
            if (idx < 0 || idx > _events.Signals.Count - 1)
                throw new IndexOutOfRangeException();

            if (_events.Signals[idx] == newKey)
                return;
            bool alreadyUsed = _events.Signals.Contains(newKey);
            if (newKey == null || _events.Signals[idx] == null || !alreadyUsed)
                _events.Signals[idx] = newKey;

            if (newKey != null && alreadyUsed)
                throw new ArgumentException("SignalAsset already used.");
        }

        /// <summary>
        /// Removes the SignalAsset and reaction at a specific index.
        /// </summary>
        /// <param name="idx">The index of the SignalAsset to be removed.</param>
        public void RemoveAtIndex(int idx)
        {
            if (idx < 0 || idx > _events.Signals.Count - 1)
                throw new IndexOutOfRangeException();
            _events.Remove(idx);
        }

        /// <summary>
        /// Replaces the reaction at a specific index with a new UnityEvent.
        /// </summary>
        /// <param name="idx">The index of the reaction to be replaced.</param>
        /// <param name="reaction">The replacement reaction.</param>
        /// <exception cref="ArgumentNullException">Thrown when the replacement reaction is null.</exception>
        public void ChangeReactionAtIndex(int idx, DataUnityEvent reaction)
        {
            if (idx < 0 || idx > _events.Events.Count - 1)
                throw new IndexOutOfRangeException();

            _events.Events[idx] = reaction;
        }

        /// <summary>
        /// Gets the reaction at a specific index.
        /// </summary>
        /// <param name="idx">The index of the reaction.</param>
        /// <returns>Returns a reaction.</returns>
        public DataUnityEvent GetReactionAtIndex(int idx)
        {
            if (idx < 0 || idx > _events.Events.Count - 1)
                throw new IndexOutOfRangeException();
            return _events.Events[idx];
        }

        /// <summary>
        /// Gets the SignalAsset at a specific index
        /// </summary>
        /// <param name="idx">The index of the SignalAsset.</param>
        /// <returns>Returns a SignalAsset.</returns>
        public SignalAsset GetSignalAssetAtIndex(int idx)
        {
            if (idx < 0 || idx > _events.Signals.Count - 1)
                throw new IndexOutOfRangeException();
            return _events.Signals[idx];
        }

        // ReSharper disable once Unity.RedundantEventFunction
        // Required by Unity for the MonoBehaviour to have an enabled state
        private void OnEnable()
        { }

        [Serializable]
        public class DataUnityEvent : UnityEvent<T>
        { }

        [Serializable]
        class DataEventKeyValue
        {
            [SerializeField]
            List<SignalAsset> _signals = new List<SignalAsset>();

            [SerializeField, CustomSignalEventDrawer]
            List<DataUnityEvent> _events = new List<DataUnityEvent>();

            public bool TryGetValue(SignalAsset key, out DataUnityEvent value)
            {
                int index = _signals.IndexOf(key);
                if (index != -1)
                {
                    value = _events[index];
                    return true;
                }

                value = null;
                return false;
            }

            public void Append(SignalAsset key, DataUnityEvent value)
            {
                _signals.Add(key);
                _events.Add(value);
            }

            public void Remove(int idx)
            {
                if (idx != -1)
                {
                    _signals.RemoveAt(idx);
                    _events.RemoveAt(idx);
                }
            }

            public void Remove(SignalAsset key)
            {
                int idx = _signals.IndexOf(key);
                if (idx != -1)
                {
                    _signals.RemoveAt(idx);
                    _events.RemoveAt(idx);
                }
            }

            public List<SignalAsset> Signals => _signals;

            public List<DataUnityEvent> Events => _events;
        }
    }
}