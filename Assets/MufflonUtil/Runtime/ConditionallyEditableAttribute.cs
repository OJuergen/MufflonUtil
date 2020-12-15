﻿using UnityEngine;

 namespace MufflonUtil
{
    public class ConditionallyEditableAttribute : PropertyAttribute
    {
        public ConditionallyEditableAttribute(string boolFieldName)
        { }

        public ConditionallyEditableAttribute(string fieldName, object fieldValue, bool invert = false)
        { }
    }
}