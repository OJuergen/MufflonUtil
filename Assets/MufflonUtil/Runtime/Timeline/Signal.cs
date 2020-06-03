using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MufflonUtil
{
    public class Signal : Marker, INotification
    {
        public virtual PropertyName id => "Signal";
    }

    public abstract class Signal<T> : Signal
    {
        public override PropertyName id => $"Signal ({typeof(T)})";
        [SerializeField] private T _data;
        public T Data => _data;
    }
}