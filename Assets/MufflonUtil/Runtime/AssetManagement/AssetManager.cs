using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace MufflonUtil
{
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
    public abstract class AssetManager<TManager, TAsset> : ScriptableObjectSingleton<TManager>
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
                throw new ArgumentException($"no asset with id {id} found");
            }

            return _assets[id];
        }

        public TAsset[] GetAll()
        {
            return _assets.ToArray();
        }

        public int GetID(TAsset asset)
        {
            if (!_idByAsset.ContainsKey(asset))
                throw new InvalidOperationException($"{asset} not found");
            return _idByAsset[asset];
        }

        private void Register(ScriptableObject asset)
        {
            if (!(asset is TAsset t) || _assets.Contains(t))
                return;

            _assets.Add(t);
            AssignIDs();
        }

        private void Unregister(ScriptableObject asset)
        {
            if (!(asset is TAsset t) || !_assets.Contains(t))
                return;

            _assets.Remove(t);
            AssignIDs();
        }

        protected new void OnEnable()
        {
            base.OnEnable();
            AssignIDs();
#if UNITY_EDITOR
            AssetPostProcessor.ImportedScriptableObject += Register;
            AssetModificationProcessor.DeletingScriptableObject += Unregister;
#endif
        }

        protected new void OnDisable()
        {
            base.OnDisable();
#if UNITY_EDITOR
            AssetPostProcessor.ImportedScriptableObject -= Register;
            AssetModificationProcessor.DeletingScriptableObject -= Unregister;
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            FindAssets();
        }
#endif

        private void AssignIDs()
        {
            _assets = _assets
                .Where(asset => asset != null)
                .Distinct()
                .OrderBy(asset => asset.name)
                .ToList();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            _idByAsset.Clear();
            for (var i = 0; i < _assets.Count; i++)
                AssignID(_assets[i], i);
            OnIDsAssigned(_assets);
        }

        private void AssignID(TAsset asset, int id)
        {
            _idByAsset[asset] = id;
            OnIDAssigned(asset, id);
        }

        protected virtual void OnIDAssigned(TAsset asset, int id)
        { }

        protected virtual void OnIDsAssigned(List<TAsset> assets)
        { }

        protected void FindAssets()
        {
            _assets = Resources.FindObjectsOfTypeAll<TAsset>()
                .Distinct()
                .OrderBy(a => a.name)
                .ToList();
            AssignIDs();
        }
    }
}