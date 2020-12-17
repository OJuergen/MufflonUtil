using System;

namespace MufflonUtil
{
    public class StringSignalHandler : SignalHandler<StringSignal, StringReaction>
    { }

    [Serializable]
    public class StringReaction : Reaction<string>
    { }
}