using System;
using UnityEngine;
using UnityEngine.Events;

namespace MufflonUtil
{
    [Serializable]
    public class BoolUnityEvent : UnityEvent<bool>
    { }
    
    [Serializable]
    public class IntUnityEvent : UnityEvent<int>
    { }
    
    [Serializable]
    public class FloatUnityEvent : UnityEvent<float>
    { }
    
    [Serializable]
    public class StringUnityEvent : UnityEvent<string>
    { }
    
    [Serializable]
    public class Vector3UnityEvent : UnityEvent<Vector3>
    { }
    
    [Serializable]
    public class QuaternionUnityEvent : UnityEvent<Quaternion>
    { }
    
    [Serializable]
    public class ColliderUnityEvent : UnityEvent<Collider>
    { }
    
    [Serializable]
    public class IntBoolUnityEvent : UnityEvent<int, bool>
    { }
    
    [Serializable]
    public class IntFloatUnityEvent : UnityEvent<int, float>
    { }
}