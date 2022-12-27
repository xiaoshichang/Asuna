using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asuna.UI
{
    
    /// <summary>
    /// 控制UIPage如何被打开
    /// </summary>
    public class ShowPageParam
    {
    }
    
    public abstract class UIPage : MonoBehaviour
    {
        public abstract void SetupController();
        public abstract void OnShow(ShowPageParam param);
        public abstract void OnHide();


        public void SetPageID(string pid)
        {
            _PageID = pid;
        }
        
        public string GetPageID()
        {
            return _PageID;
        }
        
        public void SetRoot(GameObject root)
        {
            _Root = root;
        }

        public void SetOperationHandler(AsyncOperationHandle handler)
        {
            _Handler = handler;
        }

        public AsyncOperationHandle GetOperationHandler()
        {
            return _Handler;
        }

        private string _PageID;
        private AsyncOperationHandle _Handler;
        protected GameObject _Root;
    }

}
