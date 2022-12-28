using Asuna.Application;
using Asuna.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Asuna.Scene
{
    /// <summary>
    /// 场景中的静态元素
    /// </summary>
    public class SceneItem : MonoBehaviour
    {

    }

    public static class SceneItemHelper
    {
        public static SceneItem LoadFromSceneItemDataSync(SceneItemData sid)
        {
            XDebug.Assert(sid != null);
            return null;
        }
    }
}