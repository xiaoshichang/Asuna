using System;
using System.Collections.Generic;

namespace AF.Scene
{
    public delegate void OnSceneLoaded();
    
    /// <summary>
    /// 场景的持久化数据格式
    /// </summary>
    [Serializable]
    public class SceneData
    {
        public List<SceneItemData> Items { get; set; }
    }


    /// <summary>
    /// 用于描述一次加载任务
    /// </summary>
    public class LoadSceneTask
    {
        public SceneData SceneData;
        
        /// <summary>
        /// 加载完毕回调
        /// </summary>
        public OnSceneLoaded OnLoad;
    }
}