using System.Collections;
using Asuna.Application;
using Asuna.Asset;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Entity
{
    public class ModelComponent : Component
    {
        public override void Init(Entity owner, object param)
        {
            _Owner = owner;
            _AvatarData = param as AvatarData;
        }

        public override void Release()
        {
            _AvatarData = null;
        }

        public IEnumerator LoadModelAsync()
        {
            ADebug.Assert(_AssetRequest == null);
            _AssetRequest = G.AssetManager.LoadAsset<GameObject>(_AvatarData.ModelAsset);
            yield return _AssetRequest.AsyncOperation;
            _Model = Object.Instantiate(_AssetRequest.Asset, _Owner.GetRoot().transform);
            _Model.name = "Model";
        }

        private Entity _Owner;
        private AvatarData _AvatarData;
        private AssetRequestHandler<GameObject> _AssetRequest;
        private GameObject _Model;
    }
}