using System;
using JetBrains.Annotations;
using UnityEngine;

namespace MufflonUtil
{
    /// <summary>
    /// Scriptable object asset container for a variable of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">type of the contained variable</typeparam>
    public class VariableAsset<T> : ScriptableObject
    {
        /// <summary>
        /// The value contained in this asset.
        /// </summary>
        [field: SerializeField] public T Value { get; [UsedImplicitly] private set; }

        /// <summary>
        /// Raised whenever the <see cref="Value"/> changed through <see cref="Set"/>.
        /// </summary>
        public event Action<T> ValueChanged;

        /// <summary>
        /// Set the <see cref="Value"/> of this asset to the given value.
        /// Triggers the <see cref="ValueChanged"/> event, if the value changes.
        /// </summary>
        /// <param name="value"></param>
        public void Set(T value)
        {
            if (Equals(Value, value)) return;
            Value = value;
            ValueChanged?.Invoke(value);
        }

        /// <summary>
        /// Implicitly convert this asset to its contained value.
        /// </summary>
        /// <param name="asset">This asset</param>
        /// <returns>The contained value</returns>
        public static implicit operator T(VariableAsset<T> asset)
        {
            return asset.Value;
        }
    }
}