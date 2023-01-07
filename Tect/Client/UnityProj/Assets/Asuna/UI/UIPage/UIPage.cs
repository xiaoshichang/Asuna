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

        public void SetAssetHandler(AssetRequestHandler<GameObject> asset)
        {
            _Asset = asset;
        }

        private string _PageID;
        protected GameObject _Root;
        private AssetRequestHandler<GameObject> _Asset;
    }

}
