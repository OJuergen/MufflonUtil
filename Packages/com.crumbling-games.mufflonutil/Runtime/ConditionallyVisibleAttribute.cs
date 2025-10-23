using UnityEngine;

 namespace MufflonUtil
{
    public class ConditionallyVisibleAttribute : PropertyAttribute
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable UnusedParameter.Local
        public ConditionallyVisibleAttribute(string boolFieldName)
        { }

        public ConditionallyVisibleAttribute(string fieldName, object fieldValue, bool invert = false)
        { }
    }
}