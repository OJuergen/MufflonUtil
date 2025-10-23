using UnityEngine;

 namespace MufflonUtil
{
    public class ConditionallyEditableAttribute : PropertyAttribute
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable UnusedParameter.Local
        public ConditionallyEditableAttribute(string boolFieldName)
        { }

        public ConditionallyEditableAttribute(string fieldName, object fieldValue, bool invert = false)
        { }
    }
}