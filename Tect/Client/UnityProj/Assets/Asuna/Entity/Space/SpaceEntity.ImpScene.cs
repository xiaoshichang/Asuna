using Asuna.Application;
using Asuna.Scene;
using Asuna.Utils;

namespace Asuna.Entity
{
    public enum LoadSceneState
    {
        /// <summary>
        /// 就绪状态 - 可以启动加载流程
        /// </summary>
        Ready,
        
        /// <summary>
        /// 加载流程中
        /// </summary>
        Loading,
        
        /// <summary>
        /// 加载流程完成
        /// </summary>
        Loaded,
    }
    
    public delegate void LoadSceneCallback(LoadSceneRequest request);
    
    public class LoadSceneRequest
    {
        public string ScenePath;
        public LoadSceneCallback OnSceneDataLoaded;
        public LoadSceneCallback OnSceneItemsLoaded;
    }

    
    public partial class SpaceEntity
    {
        public void LoadScene(LoadSceneRequest request)
        {
            if (_LoadSceneState != LoadSceneState.Ready)
            {
                ADebug.Warning("state is not ready!");
                return;
            }

            _LoadSceneState = LoadSceneState.Loading;
            _LoadSceneRequest = request;
            
            
        }

        private void _OnSceneDataLoaded()
        {
            _LoadSceneRequest.OnSceneDataLoaded?.Invoke(_LoadSceneRequest);
        }

        private void _OnSceneItemsLoaded()
        {
            _LoadSceneRequest.OnSceneItemsLoaded?.Invoke(_LoadSceneRequest);
        }


        private LoadSceneRequest _LoadSceneRequest;
        private LoadSceneState _LoadSceneState = LoadSceneState.Ready;
        private AScene _Scene;

    }
}