using System;

namespace MufflonUtil
{
    [Serializable]
    public enum PostPlaybackBehaviour
    {
        KeepAsIs,
        Revert,
        Active,
        Inactive
    }
}