using UnityEngine;

namespace MufflonUtil.Samples.Basics
{
    [CreateAssetMenu(fileName = "SampleAssetManager", menuName = "MufflonUtil/Samples/Sample Asset Manager")]
    public class SampleAssetManager : AssetManager<SampleAssetManager, SampleManagedAsset>
    { }
}