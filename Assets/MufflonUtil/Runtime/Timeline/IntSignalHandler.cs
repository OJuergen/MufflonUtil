using System;

namespace MufflonUtil
{
    public class IntSignalHandler : SignalHandler<IntSignal, IntReaction>
    { }

    [Serializable]
    public class IntReaction : Reaction<int>
    { }
}