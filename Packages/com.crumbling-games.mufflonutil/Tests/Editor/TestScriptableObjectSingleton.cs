using UnityEngine;

namespace MufflonUtil.Tests
{
    [CreateAssetMenu(menuName = "MufflonUtil/Samples/SO singleton")]
    public class TestScriptableObjectSingleton : ScriptableObjectSingleton<TestScriptableObjectSingleton>
    { }
}