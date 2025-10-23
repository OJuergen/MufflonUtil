using System;
using UnityEngine;

namespace MufflonUtil
{
    public static class TransformExtensions
    {
        public static string GetFullPath(this Transform transform, char delimiter = '/', string prefix = "/")
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            if (transform.parent == null) return prefix + transform.name;
            return transform.parent.GetFullPath() + delimiter + transform.name;
        }

        public static void ResetLocalPosition(this Transform transform)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.localPosition = Vector3.zero;
        }

        public static void ResetLocalRotation(this Transform transform)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.localRotation = Quaternion.identity;
        }

        public static void ResetLocalScale(this Transform transform)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.localScale = Vector3.one;
        }

        public static void ResetLocal(this Transform transform)
        {
            transform.ResetLocalPosition();
            transform.ResetLocalRotation();
            transform.ResetLocalScale();
        }
    }
}