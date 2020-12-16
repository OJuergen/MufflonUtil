using System;
using UnityEngine;

namespace MufflonUtil
{
    public class TransformSignalHandler : SignalHandler<TransformSignal, TransformReaction>
    { }

    [Serializable]
    public class TransformReaction : Reaction<Transform>
    { }
}