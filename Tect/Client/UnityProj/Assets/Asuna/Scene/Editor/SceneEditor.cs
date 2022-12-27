using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Asuna.Utils;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Asuna.Scene.Editor
{
    
    public class SceneEditor : EditorWindow
    {
        [MenuItem("Asuna/Scene/SceneEditor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            SceneEditor window = (SceneEditor)EditorWindow.GetWindow(typeof(SceneEditor));
            _InitScene();
            _CollectSceneData();
            window.Show();
        }


        private static void _InitScene()
        {
            EditorSceneManager.OpenScene(_SceneEditorScene);
        }


        #region Load
        private static void _CollectSceneData()
        {
            
        }

        private void _LoadScene()
        {
            
        }
        #endregion

        #region Export

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
            data.Asset = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            return data;
        }

        private void _ExportSceneData()
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
            var filePath = UnityEngine.Application.dataPath + "/Demo/Res/SceneData/Demo.SceneData";
            FileUtils.WriteContentToFileSync(filePath, content, true);
            XDebug.Info($"export scene data OK! {filePath}");
        }
        #endregion
       
        
        void OnGUI()
        {
            if (GUILayout.Button("Load Scene Data"))
            {
                _LoadScene();
            }
            
            if (GUILayout.Button("Export Scene Data"))
            {
                _ExportSceneData();
            }
        }

        private void OnDestroy()
        {
            EditorSceneManager.OpenScene(_MainScene);
        }

        private const string _SceneEditorScene = "Assets/AsunaFramework/Res/Scenes/SceneEditor.unity";
        private const string _MainScene = "Assets/AsunaFramework/Res/Scenes/Main.unity";
    }
}