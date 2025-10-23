using UnityEngine;

namespace MufflonUtil
{
    public static class EventTriggerExtension
    {
        public static EventTrigger EventTrigger(this GameObject gameObject)
        {
            return gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
        }

        public static EventTrigger EventTrigger(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.GetComponent<EventTrigger>() ?? monoBehaviour.gameObject.AddComponent<EventTrigger>();
        }
    }
}