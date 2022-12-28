using System;
using System.Collections;
using System.Collections.Generic;
using Asuna.Asset;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.UI
{
    public class UIManagerInitParam
    {
        public List<UIPageRegisterItem> PageRegisterItems;
    }
    
    
    public partial class UIManager : IManager
    {
        public void Init(object param)
        {
            var initParam = param as UIManagerInitParam;
            XDebug.Assert(initParam != null);
            
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
    }

}

