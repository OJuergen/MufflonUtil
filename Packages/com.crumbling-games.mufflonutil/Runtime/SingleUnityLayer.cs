using UnityEngine;

namespace MufflonUtil
{
    [System.Serializable]
    public class SingleUnityLayer
    {
        [field: SerializeField] public int LayerIndex { get; private set; }

        public void Set(int layerIndex)
        {
            if (layerIndex is > 0 and < 32)
            {
                LayerIndex = layerIndex;
            }
        }

        public int Mask => 1 << LayerIndex;
    }
}