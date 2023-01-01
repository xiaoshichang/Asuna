using System.Collections.Generic;
using Asuna.Utils;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asuna.Scene.Editor
{
    public partial class SceneEditor
    {
        private SceneItem[] _CollectSceneItems()
        {
            var collection = new List<SceneItem>();
            foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var items = go.GetComponents<SceneItem>();
                collection.AddRange(items);
            }
            return collection.ToArray();
        }

        private SceneItemData _ConvertToSceneItemData(SceneItem item)
        {
            var data = new SceneItemData();
            var go = item.gameObject;
            data.P = go.transform.position;
            data.S = go.transform.localScale;
            data.R = go.transform.localRotation;
            data.Name = go.name;
            data.Asset = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            return data;
        }

        private void _RebuildSceneItems(SceneData sceneData)
        {
            sceneData.SceneItems.Clear();
            var items = _CollectSceneItems();
            foreach (var item in items)
            {
                var data = _ConvertToSceneItemData(item);
                sceneData.SceneItems.Add(data);
            }
        }

        private void _ExportScene()
        {
            var sceneData = AssetDatabase.LoadAssetAtPath<SceneData>(_GetCurrentSelectedScenePath());
            _RebuildSceneItems(sceneData);
            EditorUtility.SetDirty(sceneData);
            XDebug.Info("Export Scene OK!");
        }
    }
}