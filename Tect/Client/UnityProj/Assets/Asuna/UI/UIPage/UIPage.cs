using Asuna.Application;
using Asuna.Asset;
using UnityEngine;

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


        public virtual void OnHide()
        {
            Destroy(gameObject);
            G.AssetManager.ReleaseAsset(_Asset);
        }

        /// <summary>
        /// called by UIManager
        /// </summary>
        public void SetPageID(string pid)
        {
            _PageID = pid;
        }
        
        public string GetPageID()
        {
            return _PageID;
        }
        
        /// <summary>
        /// called by UIManager
        /// </summary>
        public void SetRoot(GameObject root)
        {
            _Root = root;
        }

        /// <summary>
        /// called by UIManager
        /// </summary>
        public void SetAssetHandler(AssetRequestHandler<GameObject> asset)
        {
            _Asset = asset;
        }

        protected T _Seek<T>(string path) where T : class
        {
            var node = _Root.transform.Find(path);
            if (node == null)
            {
                return null;
            }

            var cmpt = node.GetComponent<T>();
            if (cmpt == null)
            {
                return null;
            }

            return cmpt;
        }

        protected GameObject _Seek(string path)
        {
            var node = _Root.transform.Find(path);
            if (node == null)
            {
                return null;
            }

            return node.gameObject;
        }

        private string _PageID;
        protected GameObject _Root;
        private AssetRequestHandler<GameObject> _Asset;
    }

}
