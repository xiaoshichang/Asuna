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
    
    public partial class SceneEditor : EditorWindow
    {
        [MenuItem("Asuna/Scene/SceneEditor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            SceneEditor window = (SceneEditor)EditorWindow.GetWindow(typeof(SceneEditor));
            _InitScene();
            window.Show();
        }


        private static void _InitScene()
        {
            EditorSceneManager.OpenScene(_SceneEditorScene);
        }

 
        void OnGUI()
        {
            if (GUILayout.Button("Load Scene Data"))
            {
                _LoadScene();
            }
            
            if (GUILayout.Button("Export Scene Data"))
            {
                _ExportScene();
            }
        }

        private void OnDestroy()
        {
            EditorSceneManager.OpenScene(_MainScene);
        }

        private const string _SceneEditorScene = "Assets/Asuna/Res/Scenes/SceneEditor.unity";
        private const string _MainScene = "Assets/Asuna/Res/Scenes/Main.unity";
    }
}