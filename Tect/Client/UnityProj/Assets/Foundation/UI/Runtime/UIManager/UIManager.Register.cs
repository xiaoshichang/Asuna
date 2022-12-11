using System;
using System.Collections.Generic;
using log4net.Repository.Hierarchy;
using UnityEngine;

namespace AsunaClient.Foundation.UI
{
    [Serializable]
    public class UIPageRegisterItem
    {
        [SerializeField] public string PageID;
        [SerializeField] public string AssetPath;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(PageID))
            {
                return false;
            }

            if (string.IsNullOrEmpty(AssetPath))
            {
                return false;
            }

            return true;
        }
    }
    
    
    public partial class UIManager
    {
        private void _RegisterPages(List<UIPageRegisterItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item.IsValid())
                {
                    XDebug.Warning($"the {i} item is invalid!");
                    continue;
                }
                
                if (_Items.ContainsKey(item.PageID))
                {
                    XDebug.Warning("duplicated page id");
                    continue;
                }

                _Items[item.PageID] = item;
            }
          
        }

        private void _UnRegisterPages()
        {
            _Items.Clear();
        }

        private readonly Dictionary<string, UIPageRegisterItem> _Items = new Dictionary<string, UIPageRegisterItem>();

    }
}