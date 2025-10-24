using UnityEngine;

namespace MufflonUtil.Samples.Basics
{
    [CreateAssetMenu(fileName = "SampleManagedAsset", menuName = "MufflonUtil/Samples/Managed Asset")]
    public class SampleManagedAsset : ScriptableObjectWithGUID, IManagedAsset
    { }
}