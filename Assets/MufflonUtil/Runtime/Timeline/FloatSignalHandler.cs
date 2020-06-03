using System;
using UnityEngine.Events;

namespace MufflonUtil
{
    public class FloatSignalHandler : SignalHandler<FloatSignal, FloatReaction>
    { }

    [Serializable]
    public class FloatReaction : UnityEvent<float>, IReaction<FloatSignal>
    {
        public void React(FloatSignal signal)
        {
            Invoke(signal.Data);
        }
    }
}