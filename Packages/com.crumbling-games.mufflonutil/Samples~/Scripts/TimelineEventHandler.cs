using UnityEngine;

namespace MufflonUtil.Samples
{
    public class TimelineEventHandler : MonoBehaviour
    {
        public void ReactToSignal(Signal signal)
        {
            Debug.Log($"Reacting to signal {signal}");
        }
        
        public void ReactToObject(Object arg)
        {
            Debug.Log($"Reacting to object signal with parameter {arg}");
        }

        public void ReactToFloat(float arg)
        {
            Debug.Log($"Reacting to float signal with parameter {arg}");
        }

        public void ReactToString(string arg)
        {
            Debug.Log($"Reacting to string signal with parameter {arg}");
        }
    }
}