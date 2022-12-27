﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Asuna.UI;
using Asuna.Utils;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        
        /// <summary>
        /// 利用反射注册所有 UIPage 的 meta 信息
        /// </summary>
        private List<UIPageRegisterItem> _CollectPageRegisterItemsFromAssemblies()
        {
            List<UIPageRegisterItem> items = new List<UIPageRegisterItem>();
            foreach (var assemblyName in ApplicationSetting.GameplayAssemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsSubclassOf(typeof(UIPage)))
                    {
                        continue;
                    }

                    var field = type.GetField("AssetPath");
                    if (field == null)
                    {
                        XDebug.Error("UIPage should contains AssetPath field");
                        continue;
                    }

                    var assetPath = field.GetValue(null) as string;
                    var item = new UIPageRegisterItem()
                    {
                        PageID = type.Name,
                        AssetPath = assetPath
                    };
                    items.Add(item);
                }
            }
            return items;
        }
        
        private void _InitUIManager()
        {
            UIManager = new UIManager();
            var param = new UIManagerInitParam()
            {
                AssetManager = AssetManager,
                PageRegisterItems = _CollectPageRegisterItemsFromAssemblies()
            };
            UIManager.Init(param);
            XDebug.Info("Init UI Manager Ok!");
        }

        private void _ReleaseUIManager()
        {
            UIManager.Release();
            UIManager = null;
            XDebug.Info("Release UI Manager Ok!");
        }

        public UIManager UIManager;
    }
}