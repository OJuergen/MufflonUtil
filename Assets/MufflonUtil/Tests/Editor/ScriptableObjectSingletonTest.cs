using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil.Tests
{
    public class ScriptableObjectSingletonTest
    {
        [Test]
        public void ShouldFindDefaultInstance()
        {
            TestScriptableObjectSingleton asset = AssetDatabase.FindAssets("t:TestScriptableObjectSingleton")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<TestScriptableObjectSingleton>)
                .Single();
            Assert.NotNull(asset);
            Assert.AreEqual(asset, TestScriptableObjectSingleton.Instance);
        }

        [Test]
        public void ShouldClearDefaultInstance()
        {
            Resources.UnloadAsset(TestScriptableObjectSingleton.Instance);
            Assert.Null(TestScriptableObjectSingleton.Instance);
        }

        [Test]
        public void ShouldFindTestInstance()
        {
            Resources.UnloadAsset(TestScriptableObjectSingleton.Instance);
            Assert.Null(TestScriptableObjectSingleton.Instance);
            var testInstance = ScriptableObject.CreateInstance<TestScriptableObjectSingleton>();
            Assert.AreEqual(testInstance, TestScriptableObjectSingleton.Instance);
        }
    }
}