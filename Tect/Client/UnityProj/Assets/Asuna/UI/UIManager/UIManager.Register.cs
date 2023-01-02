using System;
using System.Collections.Generic;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.UI
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
                    ADebug.Warning($"the {i} item is invalid!");
                    continue;
                }
                
                if (_Items.ContainsKey(item.PageID))
                {
                    ADebug.Warning("duplicated page id");
                    continue;
                }

                _Items[item.PageID] = item;
            }
          
        }

        private void _UnRegisterPages()
        {
            _Items.Clear();
        }

        private UIPageRegisterItem _GetRegisterItem(string pageID)
        {
            if (_Items.TryGetValue(pageID, out var item))
            {
                return item;
            }

            return null;
        }

        private readonly Dictionary<string, UIPageRegisterItem> _Items = new Dictionary<string, UIPageRegisterItem>();

    }
}