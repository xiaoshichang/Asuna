using System.Collections.Generic;
using Asuna.Utils;
using Newtonsoft.Json;
using UnityEditor;
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
            var transform = go.transform;
            data.P = SavedVector3.FromVector3(transform.localPosition);
            data.R = SavedQuaternion.FromQuaternion(transform.localRotation);
            data.S = SavedVector3.FromVector3(transform.localScale);
            data.Name = go.name;
            data.Asset = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            return data;
        }

        private void _ExportScene()
        {
            var items = _CollectSceneItems();
            var sceneData = new SceneData
            {
                Items = new List<SceneItemData>()
            };
            foreach (var item in items)
            {
                var data = _ConvertToSceneItemData(item);
                sceneData.Items.Add(data);
            }

            var content = JsonConvert.SerializeObject(sceneData, Formatting.Indented);
            var filePath = UnityEngine.Application.dataPath + "/Demo/Res/SceneData/Demo.SceneData.json";
            FileUtils.WriteContentToFileSync(filePath, content, true);
            XDebug.Info($"export scene data OK! {filePath}");
        }
    }
}