using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace MufflonUtil
{
    public abstract class SignalHandlerBase : MonoBehaviour
    {
        public abstract Type SignalType { get; }
        public abstract bool HasReactionTo(Signal signal);

        public abstract void AddReactionTo(Signal signal);

        public abstract void RemoveReactionTo(Signal signal);
    }

    public interface IReaction<in T> where T : Signal
    {
        void React(T signal);
    }

    public class Reaction<TData> : UnityEvent<TData>, IReaction<Signal<TData>>
    {
        public void React(Signal<TData> signal)
        {
            Invoke(signal.Data);
        }
    }

    public class SignalHandler<TSignal, TEvent> : SignalHandlerBase, INotificationReceiver,
        ISerializationCallbackReceiver
        where TSignal : Signal
        where TEvent : UnityEventBase, IReaction<TSignal>, new()
    {
        public override Type SignalType => typeof(TSignal);
        public event Action<TSignal> SignalFired;
        private Dictionary<TSignal, TEvent> _signalReactions = new Dictionary<TSignal, TEvent>();
        [SerializeField, CustomSignalEventDrawer]
        private TEvent[] _reactions;
        [SerializeField] private TSignal[] _signals;

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is TSignal signal)
            {
                if (_signalReactions.TryGetValue(signal, out TEvent reaction)) reaction.React(signal);
                SignalFired?.Invoke(signal);
            }
        }

        public override bool HasReactionTo(Signal signal)
        {
            return signal is TSignal tSignal && _signalReactions.ContainsKey(tSignal);
        }

        public override void AddReactionTo(Signal signal)
        {
            if (!(signal is TSignal tSignal))
                throw new ArgumentException($"Can only add reactions for signals of type {typeof(TSignal)}");
            if (HasReactionTo(tSignal)) return;
            _signalReactions.Add(tSignal, new TEvent());
        }

        public override void RemoveReactionTo(Signal signal)
        {
            if (!(signal is TSignal tSignal))
                throw new ArgumentException($"Can only remove reactions for signals of type {typeof(TSignal)}");
            _signalReactions.Remove(tSignal);
        }

        public void OnBeforeSerialize()
        {
            _signals = _signalReactions.Keys.ToArray();
            _reactions = _signalReactions.Values.ToArray();
        }

        public void OnAfterDeserialize()
        {
            _signalReactions = new Dictionary<TSignal, TEvent>();
            for (var i = 0; i < _signals.Length && i < _reactions.Length; i++)
            {
                if (_signals[i] != null) _signalReactions[_signals[i]] = _reactions[i];
            }

            _signalReactions = _signalReactions
                .Where(keyValuePair => keyValuePair.Key != null)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }
    }

    /// <summary>
    /// Receiver component that implements <see cref="INotificationReceiver"/> to handle <see cref="Signal"/>s.
    /// Holds a list of reaction events that take a <see cref="Signal"/> as a parameter.
    /// When receiving an <see cref="INotification"/> of type <see cref="Signal"/>, the reactions are invoked. 
    /// <br/>
    /// If the Signal carries data of a specific type, i.e., is an implementation of <see cref="Signal{T}"/>,
    /// consider using a type-specific handler. See <see cref="FloatSignalHandler"/> for reference.
    /// <br/>
    /// If you don't want to use serialized UnityEvents to handle the Signal (performance), consider implementing
    /// the <see cref="INotificationReceiver"/> interface with a custom handler component.
    /// </summary>
    public class SignalHandler : SignalHandler<Signal, SignalEvent>
    { }

    /// <summary>
    /// Serializable unity event to handle a signal with data of unknown type or without data.
    /// </summary>
    [Serializable]
    public class SignalEvent : UnityEvent<Signal>, IReaction<Signal>
    {
        public void React(Signal signal)
        {
            Invoke(signal);
        }
    }
}