using UnityEngine;

namespace MufflonUtil
{
    [System.Serializable]
    public class SingleUnityLayer
    {
        [SerializeField] private int m_LayerIndex;
        public int LayerIndex => m_LayerIndex;

        public void Set(int layerIndex)
        {
            if (layerIndex > 0 && layerIndex < 32)
            {
                m_LayerIndex = layerIndex;
            }
        }

        public int Mask => 1 << m_LayerIndex;
    }
}