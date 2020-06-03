using System;
using UnityEngine;

namespace MufflonUtil
{
    public class MinMaxAttribute : PropertyAttribute
    {
        public float Min, Max;

        public MinMaxAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
        
        public MinMaxAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    [Serializable]
    public struct MinMaxInt
    {
        public int Min;
        public int Max;
        public int Random => UnityEngine.Random.Range(Min, Max);
    }

    [Serializable]
    public struct MinMaxFloat
    {
        public float Min;
        public float Max;
        public float Random => UnityEngine.Random.Range(Min, Max);
    }
}