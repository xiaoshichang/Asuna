using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsunaClient.Foundation.UI
{
    
    /// <summary>
    /// 控制UIPage如何被打开
    /// </summary>
    public class ShowPageParam
    {
    }
    
    public abstract class UIPage
    {
        protected UIPage(string id, Object Root)
        {
            _PageID = id;
            _Root = Root;
        }
        
        public abstract string GetPageID();
        public abstract void SetupView();
        public abstract void SetupModel();
        public abstract void SetupController();
        public abstract void OnShow(ShowPageParam param);
        public abstract void OnHide();
        public abstract void OnDestroy();

        private string _PageID;
        private Object _Root;
    }

}
