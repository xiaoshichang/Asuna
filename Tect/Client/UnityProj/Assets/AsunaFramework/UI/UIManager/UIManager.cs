using System;
using System.Collections;
using System.Collections.Generic;
using AF.Asset;
using AF.Interface;
using AF.Utils;
using UnityEngine;

namespace AF.UI
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
            _InitScreenFadeBlock();
        }

        public void Release()
        {
            _ReleaseScreenFadeBlock();
            _ReleaseStack();
            _UnRegisterPages();
            _ReleaseHierarchy();
        }


        private AssetManager AssetManager;
    }

}

