using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Asuna.Scene.Editor
{
    
    public partial class SceneEditor : EditorWindow
    {
        [MenuItem("Asuna/Scene/SceneEditor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            SceneEditor window = (SceneEditor)GetWindow(typeof(SceneEditor));
            _ListAllSceneData();
            _InitEditorScene();
            window.Show();
        }

        private static void _ListAllSceneData()
        {
            var scenes = new List<string>();
            var guids = AssetDatabase.FindAssets("t:SceneData");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                scenes.Add(path);
            }

            _AllScenes = scenes.ToArray();
        }

        private static void _InitEditorScene()
        {
            EditorSceneManager.OpenScene(_SceneEditorScene);
        }


        private void OnDestroy()
        {
            EditorSceneManager.OpenScene(_MainScene);
        }

        
        private const string _SceneEditorScene = "Assets/Asuna/Res/Scenes/SceneEditor.unity";
        private const string _MainScene = "Assets/Asuna/Res/Scenes/Main.unity";
    }
}