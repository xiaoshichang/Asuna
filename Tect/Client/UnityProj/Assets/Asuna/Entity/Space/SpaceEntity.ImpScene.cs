using System.Collections;
using System.Collections.Generic;
using Asuna.Application;
using Asuna.Asset;
using Asuna.Scene;
using Asuna.Utils;
using UnityEngine;

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
        LoadSceneData,
        
        /// <summary>
        /// 加载流程中
        /// </summary>
        LoadSceneItems,
        
        /// <summary>
        /// 加载流程完成
        /// </summary>
        Loaded,
    }

    public enum LoadSceneError
    {
        StateInvalid,
        InvalidScenePath,
        LoadSceneDataFail,
    }
    
    public delegate void LoadSceneCallback(LoadSceneRequest request);
    public delegate void LoadSceneErrorCallback(LoadSceneRequest request, LoadSceneError error);
    
    public class LoadSceneRequest
    {
        public string ScenePath;
        public LoadSceneCallback OnSceneDataLoaded;
        public LoadSceneCallback OnSceneItemsLoaded;
        public LoadSceneCallback OnSceneLoaded;
        public LoadSceneErrorCallback OnError;
    }
    
    public partial class SpaceEntity
    {
        public void LoadScene(LoadSceneRequest request)
        {
            if (_LoadSceneState != LoadSceneState.Ready)
            {
                _OnLoadSceneError(LoadSceneError.StateInvalid);
                return;
            }
            if (string.IsNullOrEmpty(request.ScenePath))
            {
                _OnLoadSceneError(LoadSceneError.LoadSceneDataFail);
                return;
            }

            _LoadSceneRequest = request;
            G.CoroutineManager.StartGlobalCoroutine(_StartLoadScene());
        }

        private IEnumerator _StartLoadSceneData()
        {
            _LoadSceneState = LoadSceneState.LoadSceneData;
            _sceneDataRequestHandler = G.AssetManager.LoadAsset<SceneData>(_LoadSceneRequest.ScenePath);
            yield return _sceneDataRequestHandler.AsyncOperation;
        }
        
        private IEnumerator _StartLoadSceneItems()
        {
            var sceneData = _sceneDataRequestHandler.Asset;
            _LoadSceneState = LoadSceneState.LoadSceneItems;
            yield return null;
            foreach (var item in sceneData.SceneItems)
            {
                var sceneItemHandler = G.AssetManager.LoadAsset<GameObject>(item.Asset);
                yield return sceneItemHandler.AsyncOperation;
                _AllLoadedAssets.Add(sceneItemHandler);
                var go = Object.Instantiate(sceneItemHandler.Asset, _RootGO.transform);
                var sceneItem = go.GetComponent<SceneItem>();
                _AllLoadedSceneItems.Add(sceneItem);
                go.transform.position = item.P;
                go.transform.localRotation = item.R;
                go.transform.localScale = item.S;
                go.name = item.Name;
            }
        }

        /// <summary>
        /// 加载场景的主入口
        /// </summary>
        private IEnumerator _StartLoadScene()
        {
            yield return _StartLoadSceneData();
            if (_sceneDataRequestHandler.Asset == null)
            {
                _OnLoadSceneError(LoadSceneError.LoadSceneDataFail);
                yield break;
            }
            _OnSceneDataLoaded();
            yield return _StartLoadSceneItems();
            _OnSceneItemsLoaded();
            _OnSceneLoaded();
        }

        private void _OnLoadSceneError(LoadSceneError error)
        {
            ADebug.Error($"_OnLoadSceneError {error}");
            _LoadSceneRequest.OnError?.Invoke(_LoadSceneRequest, error);
        }

        private void _OnSceneDataLoaded()
        {
            _LoadSceneRequest.OnSceneDataLoaded?.Invoke(_LoadSceneRequest);
        }

        private void _OnSceneItemsLoaded()
        {
            _LoadSceneState = LoadSceneState.Loaded;
            _LoadSceneRequest.OnSceneItemsLoaded?.Invoke(_LoadSceneRequest);
        }

        private void _OnSceneLoaded()
        {
            _LoadSceneRequest.OnSceneLoaded?.Invoke(_LoadSceneRequest);
        }

        private void _DestroyAllLoadSceneItems()
        {
            foreach (var item in _AllLoadedSceneItems)
            {
                Object.Destroy(item.gameObject);
            }
        }

        /// <summary>
        /// 销毁所有实例后，释放所有相关资源
        /// </summary>
        private void _ReleaseAllAssets()
        {
            if (_sceneDataRequestHandler != null)
            {
                G.AssetManager.ReleaseAsset(_sceneDataRequestHandler);
                _sceneDataRequestHandler = null;
            }
            
            foreach (var asset in _AllLoadedAssets)
            {
                G.AssetManager.ReleaseAsset(asset);
            }
            _AllLoadedAssets.Clear();
        }

        /// <summary>
        /// 状态
        /// </summary>
        private LoadSceneState _LoadSceneState = LoadSceneState.Ready;
        
        /// <summary>
        /// 场景加载请求
        /// </summary>
        private LoadSceneRequest _LoadSceneRequest;
        
        /// <summary>
        /// SceneData 异步请求
        /// </summary>
        private AssetRequestHandler<SceneData> _sceneDataRequestHandler;

        /// <summary>
        /// 场景加载的所有SceneItem
        /// </summary>
        private readonly List<SceneItem> _AllLoadedSceneItems = new List<SceneItem>();

        /// <summary>
        /// 场景关联的所有资源
        /// </summary>
        private readonly HashSet<AssetRequestHandler> _AllLoadedAssets = new HashSet<AssetRequestHandler>();
        
    }
}