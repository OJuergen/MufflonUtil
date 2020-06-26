using MufflonUtil;
using UnityEngine;

namespace Samples.Scripts
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
    }
}