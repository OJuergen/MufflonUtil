namespace MufflonUtil
{
    /// <summary>
    /// An interactable object that can be blocked by others to prevent interaction.
    /// </summary>
    public interface IBlockable
    {
        /// <summary>
        /// Add an object as a blocker for interaction.
        /// </summary>
        /// <param name="blocker">the object blocking interaction</param>
        void AddBlocker(object blocker);

        /// <summary>
        /// Remove a blocker to free interaction.
        /// </summary>
        /// <param name="blocker">the object blocking interaction</param>
        void RemoveBlocker(object blocker);

        /// <summary>
        /// Returns whether or not interaction with this object is currently blocked.
        /// </summary>
        /// <returns></returns>
        bool IsBlocked();
    }
}