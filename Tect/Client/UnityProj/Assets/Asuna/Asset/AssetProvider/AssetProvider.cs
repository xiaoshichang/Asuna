using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.Asset
{
    public abstract class IAssetProvider
    {
        public const string ProviderMode_Key = "_Key_ProviderMode";
        public const string ProviderMode_Value_Editor = "editor";
        public const string ProviderMode_Value_AB = "ab";
        
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();
        
        /// <summary>
        /// 反初始化
        /// </summary>
        public abstract void Release();
        
        /// <summary>
        /// 加载资源的统一接口
        /// </summary>
        /// <param name="request"> 资源请求 </param>
        /// <typeparam name="T"> 资源类型 </typeparam>
        /// <returns> 资源请求句柄，用于决定异步或者同步加载，以及获取相应资源 </returns>
        public abstract AssetRequestHandler<T> LoadAsset<T>(AssetRequest request) where T : Object;
        
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="handler"> 资源请求句柄 </param>
        public abstract void ReleaseAsset(AssetRequestHandler handler);

        /// <summary>
        /// 输出调试信息
        /// </summary>
        public abstract void DebugInfo();
    }

}