﻿using UnityEngine;

 namespace MufflonUtil
{
    public class ConditionallyVisibleAttribute : PropertyAttribute
    {
        public ConditionallyVisibleAttribute(string boolFieldName)
        { }

        public ConditionallyVisibleAttribute(string fieldName, object fieldValue, bool invert = false)
        { }
    }
}