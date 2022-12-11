using System;
using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation.Interface;
using UnityEngine;

namespace AsunaClient.Foundation.UI
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
            if (initParam == null)
            {
                throw new Exception();
            }
            
            _RegisterPages(initParam.PageRegisterItems);
            _InitHierarchy();
        }

        public void Release()
        {
            _UnRegisterPages();
            _ReleaseHierarchy();
        }
        
    }

}

