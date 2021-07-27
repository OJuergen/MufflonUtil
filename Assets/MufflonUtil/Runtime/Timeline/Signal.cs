using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <summary>
    /// Very basic implementation of a Timeline <see cref="Marker"/> implementing
    /// the <see cref="INotification"/> interface.
    /// Use in conjunction with an implementation of an <see cref="INotificationReceiver"/> to trigger code execution
    /// from a timeline event without the need of an additional <see cref="SignalAsset"/> asset.
    /// For convenience, use <see cref="SignalHandler"/> to expose Unity events as a reaction to the signal.
    /// Use generic overrides <see cref="Signal{T}"/> and <see cref="SignalHandler{TSignal,TEvent}"/> to add
    /// data to custom signals.
    /// </summary>
    [DisplayName("Standalone Signal")]
    public class Signal : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField, Tooltip("ID for this signal for efficient comparison")]
        private string _id = "SignalID";
        private PropertyName _propertyName;
        [SerializeField, Tooltip("Use this flag to send the notification in Edit Mode.")]
        private bool _triggerInEditMode;
        [SerializeField, Tooltip("Use this flag to send the notification " +
                                 "if playback starts after the notification time.")]
        private bool _retroactive;
        [SerializeField, Tooltip("Use this flag to send the notification only once when looping.")]
        private bool _triggerOnce;

        public virtual PropertyName id => _propertyName;
        public NotificationFlags flags =>
            (_triggerInEditMode ? NotificationFlags.TriggerInEditMode : default) |
            (_retroactive ? NotificationFlags.Retroactive : default) |
            (_triggerOnce ? NotificationFlags.TriggerOnce : default);

        protected void OnEnable()
        {
            _propertyName = _id;
        }
    }

    /// <summary>
    /// <see cref="Signal"/> that carries data of type <typeparamref name="T"/>.
    /// Use in conjunction with a <see cref="SignalHandler{TSignal,TEvent}"/> implementation to expose
    /// <see cref="UnityEvent{T0}"/> reactions.
    /// Note there is a slight performance penalty for UnityEvents. Use a custom implementation of
    /// <see cref="INotificationReceiver"/> to circumvent this.
    /// </summary>
    /// <typeparam name="T">Type of data carried by this signal.</typeparam>
    public abstract class Signal<T> : Signal
    {
        [SerializeField] private T _data;
        public T Data => _data;

        public static implicit operator T(Signal<T> signal)
        {
            return signal._data;
        }
    }
}