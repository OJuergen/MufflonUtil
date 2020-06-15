using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace MufflonUtil
{
    public interface IAssetManager
    {
#if UNITY_EDITOR
        void FindAssets();
#endif
    }

    /// <summary>
    /// A database for assets of type <typeparamref name="TAsset"/>.
    /// Used for referencing <see cref="GetID"/>s over the network.
    /// Assets are assigned an id which can be retrieved using <see cref="Get"/>.
    /// In turn, the ID can be used to retrieve the asset using <see cref="ScriptableObjectSingleton"/>.
    /// <br/>
    /// Implements <see cref="ScriptableObjectSingleton"/>.
    /// </summary>
    /// <typeparam name="TManager">The type of the manager class. Needed for the <see cref="ScriptableObjectSingleton"/> implementation.</typeparam>
    /// <typeparam name="TAsset">The type of the managed assets.</typeparam>
    public abstract class AssetManager<TManager, TAsset> : ScriptableObjectSingleton<TManager>, IAssetManager
        where TAsset : ManagedAsset where TManager : AssetManager<TManager, TAsset>
    {
        [SerializeField] private List<TAsset> _assets = new List<TAsset>();
        private readonly Dictionary<TAsset, int> _idByAsset = new Dictionary<TAsset, int>();

        /// <summary>
        /// Retrieves an asset by its ID assigned by this manager.
        /// </summary>
        /// <param name="id">ID of managed asset</param>
        /// <returns>Asset with the given ID</returns>
        /// <exception cref="ArgumentException">Throws if no asset with the given ID is registered</exception>
        [NotNull] public TAsset Get(int id)
        {
            if (id < 0 || id >= _assets.Count)
            {
                throw new ArgumentOutOfRangeException($"no asset with id {id} found");
            }

            return _assets[id];
        }

        public TAsset[] GetAll()
        {
            return _assets.ToArray();
        }

        public int GetID(TAsset asset)
        {
            return _idByAsset.TryGetValue(asset, out int id)
                ? id
                : throw new InvalidOperationException($"{asset} not found");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += FindAssets;
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            AssetPostProcessor.ImportedScriptableObject += OnImportedScriptableObject;
            AssetPostProcessor.DeletedAsset += OnDeletedScriptableObject;
            FindAssets();
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            AssetPostProcessor.ImportedScriptableObject -= OnImportedScriptableObject;
            AssetPostProcessor.DeletedAsset -= OnDeletedScriptableObject;
        }

        private void OnImportedScriptableObject(ScriptableObject asset)
        {
            if (asset is TAsset t && !_assets.Contains(t)) FindAssets();
        }

        private void OnDeletedScriptableObject(string path)
        {
            _assets = _assets.Where(asset => asset != null).ToList();
            RefreshIdByAsset();
        }

        public void FindAssets()
        {
            _assets = UnityEditor.AssetDatabase
                .FindAssets("t:ManagedAsset")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<ManagedAsset>)
                .OfType<TAsset>()
                .OrderBy(a => a.name)
                .ToList();
            RefreshIdByAsset();
        }
#else
        protected new void OnEnable()
        {
            base.OnEnable();
            RefreshIdByAsset();
        }
#endif

        private void RefreshIdByAsset()
        {
            _idByAsset.Clear();
            for (var id = 0; id < _assets.Count; id++) _idByAsset[_assets[id]] = id;
        }
    }
}