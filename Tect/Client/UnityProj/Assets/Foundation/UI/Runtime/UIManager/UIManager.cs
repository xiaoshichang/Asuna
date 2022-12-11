using System;
using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation.Asset;
using AsunaClient.Foundation.Interface;
using UnityEngine;

namespace AsunaClient.Foundation.UI
{
    public class UIManagerInitParam
    {
        public AssetManager AssetManager;
        public List<UIPageRegisterItem> PageRegisterItems;
    }
    
    
    public partial class UIManager : IManager
    {
        public void Init(object param)
        {
            var initParam = param as UIManagerInitParam;
            XDebug.Asset(initParam != null);
            XDebug.Asset(initParam.AssetManager != null);
            
            AssetManager = initParam.AssetManager;
            
            _RegisterPages(initParam.PageRegisterItems);
            _InitHierarchy();
        }

        public void Release()
        {
            _ReleaseStack();
            _UnRegisterPages();
            _ReleaseHierarchy();
        }


        private AssetManager AssetManager;
    }

}

