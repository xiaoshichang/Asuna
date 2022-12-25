using System;
using AF.Utils;
using AF.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;


namespace AF.Asset
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
        public AsyncOperationHandle<T> LoadAssetSync<T>(string assetPath) where T : Object
        {
            var op = Addressables.LoadAssetAsync<T>(assetPath);
            op.WaitForCompletion();
            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                XDebug.Error($"load asset {assetPath} exception {op.Status}!");
            }
            
            return op;
        }

        public void LoadAssetAsync<T>()
        {
            throw new NotImplementedException();
        }

        private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            throw new NotImplementedException();
        }

        public void ReleaseAsset(AsyncOperationHandle handler)
        {
            Addressables.Release(handler);
        }
        
    }
}


