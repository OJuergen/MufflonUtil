namespace MufflonUtil
{
    public interface IDirtyMaskProvider
    {
        void SetDirtyAt(byte bitIndex);
        void SetDirty();
        bool IsDirty();
        bool IsDirtyAt(byte bitIndex);
        bool IsDirtyAt(byte dirtyMask, byte bitIndex);
    }
}