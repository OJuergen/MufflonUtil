using System;

namespace MufflonUtil
{
    /// <summary>
    /// An interactable object that can be blocked by others to prevent interaction until all blockers are removed.
    /// </summary>
    public interface IBlockable
    {
        event Action Blocked;
        event Action Unblocked;
        
        /// <summary>
        /// Returns whether or not interaction with this object is currently blocked.
        /// </summary>
        /// <returns>True iff the object is blocked.</returns>
        bool IsBlocked { get; }
        
        /// <summary>
        /// Add an object as a blocker for interaction.
        /// </summary>
        /// <param name="blocker">The object blocking interaction.</param>
        void AddBlocker(object blocker);

        /// <summary>
        /// Remove a blocker to free interaction.
        /// </summary>
        /// <param name="blocker">The object blocking interaction.</param>
        void RemoveBlocker(object blocker);
    }
}