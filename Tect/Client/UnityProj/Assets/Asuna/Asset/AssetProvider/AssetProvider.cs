using UnityEditor;
using Object = UnityEngine.Object;

namespace Asuna.Asset
{
    public abstract class IAssetProvider
    {
        public abstract void Init();
        public abstract void Release();
        public abstract T LoadAssetSync<T>(string assetPath) where T : Object;
    }

}