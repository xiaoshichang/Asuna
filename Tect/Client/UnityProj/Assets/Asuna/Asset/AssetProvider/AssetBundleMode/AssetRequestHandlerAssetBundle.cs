using System.Collections;
using Asuna.Foundation.Debug;
using UnityEngine;


namespace Asuna.Asset
{
    public class AssetRequestHandlerAssetBundle<T> : AssetRequestHandler<T> where T : Object
    {
        public AssetRequestHandlerAssetBundle(AssetRequest request, AssetProviderAssetBundle provider) : base(request)
        {
            _Provider = provider;
            _AssetID = provider.AssetPathToAssetID(request.AssetPath);
            _AssetBundleName = provider.GetAssetBundleByAssetID(_AssetID);
        }

        public override T SyncLoad()
        {
            if (Asset is not null)
            {
                return Asset;
            }
            if (!_Provider.IsAssetBundleLoaded(_AssetBundleName))
            {
                _RuntimeAssetBundle = _Provider.LoadAssetBundleSync(_AssetBundleName);
            }
            else
            {
                _RuntimeAssetBundle = _Provider.GetRuntimeAssetBundle(_AssetBundleName);
            }
            Asset = _RuntimeAssetBundle.LoadAssetSync<T>(_AssetID);
            _RuntimeAssetBundle.IncRefCounter();
            return Asset;
        }
        
        private IEnumerator _LoadAssetAsync()
        {
            if (Asset is not null)
            {
                yield break;
            }
            yield return _Provider.LoadAssetBundleAsync(_AssetBundleName);
            _RuntimeAssetBundle = _Provider.GetRuntimeAssetBundle(_AssetBundleName);
            if (_RuntimeAssetBundle == null)
            {
                ADebug.Error($"{_AssetBundleName} not loaded!");
                yield break;
            }
            var nativeRequest = _RuntimeAssetBundle.LoadAssetAsync<T>(_AssetID);
            yield return nativeRequest;
            if (nativeRequest.asset == null)
            {
                ADebug.Error($"{_Request.AssetPath} not loaded!");
                yield break;
            }
            _RuntimeAssetBundle.IncRefCounter();
            Asset = nativeRequest.asset as T;
        }
        
        public override void OnReleaseAsset()
        {
            if (_RuntimeAssetBundle == null || Asset is null)
            {
                ADebug.Error("release before load");
                return;
            }
            _RuntimeAssetBundle.DecRefCounter();
        }
        
        private RuntimeAssetBundle _RuntimeAssetBundle;
        public override IEnumerator AsyncOperation => _LoadAssetAsync();
        public override T Asset  { get; protected set; }
        private readonly string _AssetID;
        private readonly string _AssetBundleName;
        private readonly AssetProviderAssetBundle _Provider;
    }
}