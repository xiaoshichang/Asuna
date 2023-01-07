using System;
using System.Collections.Generic;
using System.Linq;
using Asuna.Application;
using Asuna.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.UI
{
    public partial class UIManager
    {
        private UIPage _CreatePage(UIPageRegisterItem item)
        {
            var handler = G.AssetManager.LoadAsset<GameObject>(item.AssetPath);
            handler.SyncLoad();
            var root = Object.Instantiate(handler.Asset, _PageRoot.transform);
            
            var page = root.GetComponent<UIPage>();
            if (page is null)
            {
                ADebug.Error("root node must contains a UIPage script!");
                return null;
            }
            
            page.SetPageID(item.PageID);
            page.SetRoot(root);
            page.SetAssetHandler(handler);
            page.SetupController();
            
            return page;
        }

        private void _TryHideTop()
        {
            if (_PageStack.Count == 0)
            {
                return;
            }

            var page = _PageStack.Last();
            page.gameObject.SetActive(false);
        }

        private void _TryShowTop()
        {
            if (_PageStack.Count == 0)
            {
                return;
            }
            var page = _PageStack.Last();
            page.gameObject.SetActive(true);
        }

        /// <summary>
        /// 判断某个UIPage是否在栈中
        /// </summary>
        private bool _IsPageInStack(string pageID)
        {
            foreach(var page in _PageStack)
            {
                if (pageID == page.GetPageID())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 从UI栈底部提升到顶部
        /// </summary>
        private void _PromotePage(string pageID, ShowPageParam param)
        {
            // todo
            throw new NotImplementedException();
        }

        /// <summary>
        /// 构造Page并入栈
        /// </summary>
        private void _DoShowPage(string pageID, ShowPageParam param)
        {
            var registerItem = _GetRegisterItem(pageID);
            if (registerItem == null)
            {
                ADebug.Error("page id unknown!");
                return;
            }

            var page = _CreatePage(registerItem);
            if (page is null)
            {
                ADebug.Error($"load page fail! id : {registerItem.PageID}, path: {registerItem.AssetPath}");
                return;
            }

            _TryHideTop();
            _PageStack.Add(page);
            page.OnShow(param);
        }
        
        /// <summary>
        /// 显示UI Page的统一入口
        /// </summary>
        public void ShowPage(string pageID, ShowPageParam param)
        {
            if (_IsPageInStack(pageID))
            {
                _PromotePage(pageID, param);
            }
            else
            {
                _DoShowPage(pageID, param);
            }
        }

        public void HidePage(string pageID)
        {
            var page = _GetPageByID(pageID);
            if (page is null)
            {
                ADebug.Warning($"Page {pageID} not found");
                return;
            }
            
            page.OnHide();
            _PageStack.Remove(page);
            _TryShowTop();
        }

        private void _ReleaseStack()
        {
            while (_PageStack.Count != 0)
            {
                var page = _PageStack.Last();
                page.OnHide();
                _PageStack.RemoveAt(_PageStack.Count - 1);
            }
        }

        private UIPage _GetPageByID(string pageID)
        {
            foreach (var page in _PageStack)
            {
                if (page.GetPageID() == pageID)
                {
                    return page;
                }
            }
            return null;
        }

        private readonly List<UIPage> _PageStack = new List<UIPage>();
    }
}