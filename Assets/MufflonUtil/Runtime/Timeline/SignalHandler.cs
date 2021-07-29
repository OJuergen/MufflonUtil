using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace MufflonUtil
{
    /// <summary>
    /// Internal base class. Do NOT inherit from this! Inherit from <see cref="SignalHandler{TSignal,TEvent}"/> instead.
    /// This non-generic base class is needed to allow for custom inspectors.
    /// </summary>
    public abstract class SignalHandlerBase : MonoBehaviour
    {
        public abstract Type SignalType { get; }
        public abstract bool HasReactionTo(Signal signal);

        public abstract void AddReactionTo(Signal signal);

        public abstract void RemoveReactionTo(Signal signal);
        public abstract bool HasReactionTo(string group);

        public abstract void AddReactionTo(string group);

        public abstract void RemoveReactionTo(string group);
    }

    /// <summary>
    /// Base class for signal handlers. Implement this to handle Timeline markers of type <typeparamref name="TSignal"/>.
    /// Exposes serializable UnityEvents of type <typeparamref name="TEvent"/> that pass the data of the signal.
    /// </summary>
    /// <typeparam name="TSignal">The <see cref="Signal"/> type. Must be [Serializable].</typeparam>
    /// <typeparam name="TEvent">The Unity event to handle the signal. Must be [Serializable]</typeparam>
    public abstract class SignalHandler<TSignal, TEvent> : SignalHandlerBase, INotificationReceiver,
        ISerializationCallbackReceiver
        where TSignal : Signal
        where TEvent : UnityEventBase, new()
    {
        public override Type SignalType => typeof(TSignal);
        private Dictionary<TSignal, TEvent> _signalReactions = new Dictionary<TSignal, TEvent>();
        private Dictionary<string, TEvent> _signalGroupReactions = new Dictionary<string, TEvent>();
        [SerializeField] private TSignal[] _signals;
        [SerializeField, CustomSignalEventDrawer]
        private TEvent[] _reactions;
        [SerializeField, SignalGroup(false)] private string[] _signalGroups;
        [SerializeField, CustomSignalEventDrawer]
        private TEvent[] _groupReactions;

        // ReSharper disable once Unity.RedundantEventFunction - Needed for Unity to allow enabling/disabling this
        private void OnEnable()
        { }

        public virtual void OnNotify(Playable origin, INotification notification, object context)
        {
            if (!enabled) return;
            if (notification is TSignal signal)
            {
                if (_signalReactions.TryGetValue(signal, out TEvent reaction)) React(signal, reaction);
                if (_signalGroupReactions.TryGetValue(signal.Group, out TEvent groupReaction))
                    React(signal, groupReaction);
                RaiseEvent(signal);
            }
        }

        protected abstract void React(TSignal signal, TEvent reaction);
        protected abstract void RaiseEvent(TSignal signal);

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

        public override bool HasReactionTo(string signalGroup)
        {
            return _signalGroupReactions.ContainsKey(signalGroup);
        }

        public override void AddReactionTo([NotNull] string group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            if (HasReactionTo(group)) return;
            _signalGroupReactions.Add(group, new TEvent());
        }

        public override void RemoveReactionTo([NotNull] string group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            _signalGroupReactions.Remove(group);
        }

        public void OnBeforeSerialize()
        {
            _signals = _signalReactions.Keys.ToArray();
            _reactions = _signalReactions.Values.ToArray();
            _signalGroups = _signalGroupReactions.Keys.ToArray();
            _groupReactions = _signalGroupReactions.Values.ToArray();
        }

        public void OnAfterDeserialize()
        {
            _signalReactions = new Dictionary<TSignal, TEvent>();
            for (var i = 0; i < _signals.Length && i < _reactions.Length; i++)
            {
                if (_signals[i] != null) _signalReactions[_signals[i]] = _reactions[i];
            }

            _signalGroupReactions = new Dictionary<string, TEvent>();
            for (var i = 0; i < _signalGroups.Length && i < _groupReactions.Length; i++)
            {
                if (_signalGroups[i] != null) _signalGroupReactions[_signalGroups[i]] = _groupReactions[i];
            }
        }
    }

    /// <summary>
    /// Receiver component that implements <see cref="INotificationReceiver"/> to handle data-carrying
    /// <see cref="Signal{T}"/>s. Holds a list of reaction <see cref="UnityEvent{T}"/>.
    /// When receiving an <see cref="INotification"/> of type <see cref="Signal{T}"/>, the reactions are invoked. 
    /// There is also a plain C# event <see cref="Fired"/> that can be subscribed to.
    /// </summary>
    public class DataSignalHandler<T> : SignalHandler<Signal<T>, UnityEvent<T>>
    {
        public event Action<T> Fired;

        protected override void React(Signal<T> signal, UnityEvent<T> reaction)
        {
            reaction?.Invoke(signal.Data);
        }

        protected override void RaiseEvent(Signal<T> signal)
        {
            Fired?.Invoke(signal.Data);
        }
    }

    /// <summary>
    /// Receiver component that implements <see cref="INotificationReceiver"/> to handle <see cref="Signal"/>s.
    /// Holds a list of reaction <see cref="UnityEvent{T}"/>s.
    /// When receiving an <see cref="INotification"/> of type <see cref="Signal"/>, the reactions are invoked
    /// and the signal is passed as an event parameter.
    /// <br/>
    /// If the Signal carries data of a specific type, i.e., is an implementation of <see cref="Signal{T}"/>,
    /// consider using an implementation of <see cref="DataSignalHandler{T}"/> instead.
    /// <br/>
    /// There is also a plain C# event <see cref="Fired"/> that can be subscribed to.
    /// </summary>
    public class SignalHandler : SignalHandler<Signal, UnityEvent<Signal>>
    {
        public event Action<Signal> Fired;

        protected override void React(Signal signal, UnityEvent<Signal> reaction)
        {
            reaction?.Invoke(signal);
        }

        protected override void RaiseEvent(Signal signal)
        {
            Fired?.Invoke(signal);
        }
    }
}