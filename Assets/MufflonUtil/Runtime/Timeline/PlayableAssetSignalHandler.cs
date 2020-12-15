using System;
using UnityEngine.Playables;

namespace MufflonUtil
{
    [Serializable]
    public class PlayableAssetReaction : Reaction<PlayableAsset>
    { }

    public class PlayableAssetSignalHandler : SignalHandler<PlayableAssetSignal, PlayableAssetReaction>
    { }
}