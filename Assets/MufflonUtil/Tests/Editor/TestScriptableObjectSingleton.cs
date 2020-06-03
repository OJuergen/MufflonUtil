using UnityEngine;

namespace MufflonUtil.Tests
{
    [CreateAssetMenu(menuName = "Util/Tests/SO singleton")]
    public class TestScriptableObjectSingleton : ScriptableObjectSingleton<TestScriptableObjectSingleton>
    { }
}