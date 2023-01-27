using System.Collections;
using Asuna.Application;
using Asuna.Asset;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Entity
{
    public class ModelComponentInitParam
    {
        public string ModelAssetPath;
    }
    
    
    public class ModelComponent : Component
    {
        public override void Init(Entity owner, object param)
        {
            _Owner = owner as AvatarEntity;
            _InitParam = param as ModelComponentInitParam;
            ADebug.Assert(_Owner != null);
        }

        public override void Release()
        {
            if (_AssetRequest != null)
            {
                G.AssetManager.ReleaseAsset(_AssetRequest);
            }
        }
        
        public IEnumerator LoadModelAsync()
        {
            ADebug.Assert(_AssetRequest == null);
            _AssetRequest = G.AssetManager.LoadAsset<GameObject>(_Owner.AvatarInitParam.ModelComponentInitParam.ModelAssetPath);
            yield return _AssetRequest.AsyncOperation;
            _Model = Object.Instantiate(_AssetRequest.Asset, _Owner.GetRootGO().transform);
            _Model.name = "Model";
        }

        private AvatarEntity _Owner;
        private ModelComponentInitParam _InitParam;
        private AssetRequestHandler<GameObject> _AssetRequest;
        private GameObject _Model;
    }
}