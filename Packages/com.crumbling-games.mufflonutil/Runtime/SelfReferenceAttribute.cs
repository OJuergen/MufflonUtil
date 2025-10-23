using UnityEngine;

namespace MufflonUtil
{
    public class SelfReferenceAttribute : PropertyAttribute
    {
        // ReSharper disable twice UnusedParameter.Local
        public SelfReferenceAttribute(bool findInChildren = false, bool findInParent = false, bool autoAssign = true)
        { }
    }
}