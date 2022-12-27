﻿using System.Collections.Generic;

namespace Asuna.Scene
{
    public delegate void OnSceneLoadedCallback(LoadSceneTask task);
    
    /// <summary>
    /// 场景的静态部分
    /// </summary>
    public class Scene
    {
        public void Load(LoadSceneTask task)
        {
            _CurrentTask = task;
        }

        /// <summary>
        /// 当前加载任务
        /// </summary>
        private LoadSceneTask _CurrentTask;
        
        /// <summary>
        /// 所有场景项
        /// </summary>
        private List<SceneItem> _SceneItems = new List<SceneItem>();
    }
}