using UnityEditor;
using UnityEngine;

namespace Asuna.Scene.Editor
{
    public static class SceneItemCreator
    {
        [MenuItem("Asuna/Scene/CreateSceneItem")]
        public static void CreateSceneItem()
        {
            var go = new GameObject($"SceneItem-{_EmptySceneItemCounter}");
            _EmptySceneItemCounter++;
            var sceneItem = go.AddComponent<SceneItem>();
            var art = new GameObject("Art");
            art.transform.SetParent(go.transform);
        }
        
        private static int _EmptySceneItemCounter = 1;

    }
}