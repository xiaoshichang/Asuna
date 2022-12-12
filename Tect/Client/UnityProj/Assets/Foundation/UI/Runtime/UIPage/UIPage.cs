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

        public void SetAsset(GameObject asset)
        {
            _Asset = asset;
        }

        public GameObject GetAsset()
        {
            return _Asset;
        }

        private string _PageID;
        private GameObject _Asset;
        protected GameObject _Root;
    }

}
