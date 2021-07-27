using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    /// <inheritdoc cref="UnityEngine.Timeline.IMarker" />
    /// <summary>
    /// Marker that emits a signal and passes data to a DataSignalReceiver.
    /// </summary>
    /// A DataSignalEmitter emits a notification that holds data through the playable system.
    /// A DataSignalEmitter is used with a DataSignalReceiver and a SignalAsset.
    /// <seealso cref="UnityEngine.Timeline.SignalAsset"/>
    /// <seealso cref="DataSignalReceiver{T}"/>
    public abstract class DataSignalEmitter<T> : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField, Tooltip("ID for this signal for efficient comparison")]
        private string _id = "SignalID";
        private PropertyName _propertyName;
        [SerializeField, Tooltip("Data passed to the signal receiver")] private T _data;
        public T Data => _data;
        [SerializeField, Tooltip("Use this flag to send the notification in Edit Mode.")]
        private bool _triggerInEditMode = true;
        [SerializeField, Tooltip("Use this flag to send the notification " +
                                 "if playback starts after the notification time.")]
        private bool _retroactive;
        [SerializeField, Tooltip("Use this flag to send the notification only once when looping.")]
        private bool _triggerOnce;
        [SerializeField, Tooltip("The signal asset that connects emitter and receiver.")] SignalAsset _signalAsset;
        public SignalAsset SignalAsset => _signalAsset;

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
}