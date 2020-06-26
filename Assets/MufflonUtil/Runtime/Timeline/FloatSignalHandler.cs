using System;

namespace MufflonUtil
{
    public class FloatSignalHandler : SignalHandler<FloatSignal, FloatReaction>
    { }

    [Serializable]
    public class FloatReaction : Reaction<float>
    { }
}