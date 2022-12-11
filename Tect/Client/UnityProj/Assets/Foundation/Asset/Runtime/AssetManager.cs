using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation.Interface;
using UnityEngine;


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

        public T LoadAsset<T>(string assetPath) where T : class
        {
            return null;
        }

        public void ReleaseAsset(GameObject asset)
        {
        }
        
    }
}


