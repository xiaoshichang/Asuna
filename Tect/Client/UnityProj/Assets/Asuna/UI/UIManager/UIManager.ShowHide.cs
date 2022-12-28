using System;
using System.Collections.Generic;
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
            var asset = G.AssetManager.LoadAssetSync<GameObject>(item.AssetPath);
            var root = Object.Instantiate(asset, _PageRoot.transform);
            
            var page = root.GetComponent<UIPage>();
            if (page == null)
            {
                XDebug.Error("root node must contains a UIPage script!");
                return null;
            }
            
            page.SetPageID(item.PageID);
            page.SetRoot(root);
            page.SetupController();
            
            return page;
        }

        private void _TryHideTop()
        {
            if (_PageStack.TryPeek(out var top))
            {
                top.gameObject.SetActive(false);
            }
        }

        private void _TryShowTop()
        {
            if (_PageStack.TryPeek(out var top))
            {
                top.gameObject.SetActive(true);
            }
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
                XDebug.Error("page id unknown!");
                return;
            }

            var page = _CreatePage(registerItem);
            if (page == null)
            {
                XDebug.Error($"load page fail! id : {registerItem.PageID}, path: {registerItem.AssetPath}");
                return;
            }
            _TryHideTop();
            
            page.OnShow(param);
            _PageStack.Push(page);
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

        public void HidePage()
        {
            if (_PageStack.Count == 0)
            {
                XDebug.Warning("pop while stack is empty");
                return;
            }

            var page = _PageStack.Pop();
            page.OnHide();
            _TryShowTop();
        }

      

        private void _ReleaseStack()
        {
            while (_PageStack.Count != 0)
            {
                var page = _PageStack.Pop();
                page.OnHide();
            }
        }

        private readonly Stack<UIPage> _PageStack = new Stack<UIPage>();
    }
}