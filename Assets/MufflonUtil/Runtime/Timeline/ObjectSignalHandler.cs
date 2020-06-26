using System;

namespace MufflonUtil
{
    public class ObjectSignalHandler : SignalHandler<ObjectSignal, ObjectReaction>
    { }

    [Serializable]
    public class ObjectReaction : Reaction<UnityEngine.Object>
    { }
}