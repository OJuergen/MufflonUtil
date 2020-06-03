namespace MufflonUtil
{
    public interface IBlockable
    {
        void AddBlocker(object blocker);
        void RemoveBlocker(object blocker);
    }
}