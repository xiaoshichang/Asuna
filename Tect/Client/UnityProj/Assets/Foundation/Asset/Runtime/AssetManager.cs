using System;
using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;


namespace AsunaClient.Foundation.Asset
{
    public class AssetManager : IManager
    {
        public void Init(object param)
        {
        }

        public void Release()
        {
        }
        
        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.addressables@1.21/manual/SynchronousAddressables.html
        /// </summary>
        public T LoadAssetSync<T>(string assetPath) where T : Object
        {
            var op = Addressables.LoadAssetAsync<T>(assetPath);
            T asset = op.WaitForCompletion();

            if (op.Status == AsyncOperationStatus.Failed)
            {
                return null;
            }
            
            return asset;
        }

        public void LoadAssetAsync<T>()
        {
            throw new NotImplementedException();
        }

        private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            throw new NotImplementedException();
        }

        public void ReleaseAsset(Object asset)
        {
            Addressables.Release(asset);
        }
        
    }
}


