using System;

namespace MufflonUtil
{
    public class IntSignalHandler : SignalHandler<FloatSignal, IntReaction>
    { }

    [Serializable]
    public class IntReaction : Reaction<float>
    { }
}