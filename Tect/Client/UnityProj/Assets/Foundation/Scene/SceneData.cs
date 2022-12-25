namespace AsunaClient.Foundation.Scene
{
    public delegate void OnSceneLoaded();
    
    /// <summary>
    /// 用于描述场景的逻辑数据
    /// </summary>
    public class SceneData
    {
        
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