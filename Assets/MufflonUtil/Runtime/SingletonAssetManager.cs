using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MufflonUtil
{
    public abstract class SingletonAssetManager<TManager, TAsset> : AssetManager<TManager, TAsset>
        where TManager : SingletonAssetManager<TManager, TAsset> where TAsset : ManagedAsset
    {
        private Dictionary<Type, TAsset> _assetByType = new Dictionary<Type, TAsset>();
        private bool _hasError;

        public T Get<T>() where T : TAsset
        {
            return (T) _assetByType[typeof(T)];
        }

        protected override void OnIDsAssigned(List<TAsset> assets)
        {
            base.OnIDsAssigned(assets);
            try
            {
                _assetByType = assets.ToDictionary(gameAction => gameAction.GetType(), gameAction => gameAction);
                if (_hasError)
                {
                    Debug.Log($"Conflict resolved for {name}!");
                    _hasError = false;
                }
            }
            catch (ArgumentException)
            {
                Debug.LogError($"{name} registered multiple managed singleton assets with the same type.\n"
                               + "This is not supported. Please make sure, there is only one asset per type.");
                _hasError = true;
            }
        }
    }
}