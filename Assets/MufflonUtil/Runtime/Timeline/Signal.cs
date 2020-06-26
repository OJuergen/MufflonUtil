using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    public class Signal : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField, Tooltip("Use this flag to send the notification in Edit Mode.")]
        private bool _triggerInEditMode;
        [SerializeField, Tooltip("Use this flag to send the notification " +
                                 "if playback starts after the notification time.")]
        private bool _retroactive;
        [SerializeField, Tooltip("Use this flag to send the notification only once when looping.")]
        private bool _triggerOnce;

        public virtual PropertyName id => "Signal";
        public NotificationFlags flags =>
            (_triggerInEditMode ? NotificationFlags.TriggerInEditMode : default) |
            (_retroactive ? NotificationFlags.Retroactive : default) |
            (_triggerOnce ? NotificationFlags.TriggerOnce : default);
    }

    public abstract class Signal<T> : Signal
    {
        public override PropertyName id => $"Signal ({typeof(T)})";
        [SerializeField] private T _data;
        public T Data => _data;

        public static implicit operator T(Signal<T> signal)
        {
            return signal._data;
        }
    }
}