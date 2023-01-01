using Asuna.Utils;
using UnityEditor;
using UnityEngine;

namespace Asuna.Scene.Editor
{
    public partial class SceneEditor
    {
        private void _LoadScene()
        {
            var path = _AllScenes[_CurrentSceneIdx];
            var sceneData = AssetDatabase.LoadAssetAtPath<SceneData>(path);
            
            _InitEditorScene();
            _LoadSceneDataToEditor(sceneData);
        }

        private void _LoadSceneItemData(SceneItemData itemData)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(itemData.Asset);
            var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (go == null)
            {
                XDebug.Error("unknown prefab");
                return;
            }
            go.transform.position = itemData.P;
            go.transform.localRotation = itemData.R;
            go.transform.localScale = itemData.S;
            go.name = itemData.Name;
        }
        
        private void _LoadSceneDataToEditor(SceneData sceneData)
        {
            foreach (var itemData in sceneData.SceneItems)
            {
                _LoadSceneItemData(itemData);
            }
        }
    }
}