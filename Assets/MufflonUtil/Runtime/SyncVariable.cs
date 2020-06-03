using System.Collections.Generic;

namespace MufflonUtil
{
    public class SyncVariable<T>
    {
        private T _value;
        private readonly IDirtyMaskProvider _dirtyMaskProvider;
        private readonly byte _bitIndex;

        public SyncVariable(IDirtyMaskProvider dirtyMaskProvider, byte bitIndex, T value = default)
        {
            _value = value;
            _dirtyMaskProvider = dirtyMaskProvider;
            _bitIndex = bitIndex;
        }

        public void SetValue(T value)
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                _dirtyMaskProvider.SetDirtyAt(_bitIndex);
            }
        }

        public bool IsDirty(byte dirtyMask)
        {
            return _dirtyMaskProvider.IsDirtyAt(_bitIndex, dirtyMask);
        }

        public T GetValue()
        {
            return _value;
        }

        public static implicit operator T(SyncVariable<T> syncVar)
        {
            return syncVar.GetValue();
        }
    }
}