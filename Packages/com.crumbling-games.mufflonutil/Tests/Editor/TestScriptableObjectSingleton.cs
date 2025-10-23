using UnityEngine;

namespace MufflonUtil.Editor.Tests
{
    [CreateAssetMenu(menuName = "MufflonUtil/Samples/SO singleton")]
    public class TestScriptableObjectSingleton : ScriptableObjectSingleton<TestScriptableObjectSingleton>
    { }
}