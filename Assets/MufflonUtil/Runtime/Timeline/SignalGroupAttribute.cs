using UnityEngine;

 namespace MufflonUtil
{
    public class SignalGroupAttribute : PropertyAttribute
    {
        public bool CanAddGroup;

        public SignalGroupAttribute(bool canAddGroup)
        {
            CanAddGroup = canAddGroup;
        }
    }
}