#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Asuna.Asset
{
    public class AssetRequestHandlerEditorSim<T> : AssetRequestHandler<T> where T : Object
    {
        public AssetRequestHandlerEditorSim(AssetRequest request) : base(request)
        {
        }

        public override T SyncLoad()
        {
            return AssetDatabase.LoadAssetAtPath<T>(_Request.AssetPath);
        }
        
        private IEnumerator _Sim()
        {
            Asset = SyncLoad();
            yield return null;
        }

        public override void OnReleaseAsset()
        {
        }

        public override IEnumerator AsyncOperation => _Sim();
        public override T Asset { get; protected set; }
    }
}
#endif