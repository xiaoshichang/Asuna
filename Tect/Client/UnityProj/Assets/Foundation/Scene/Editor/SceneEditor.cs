using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace  AsunaClient.Foundation.Scene.Editor
{
    
    public class SceneEditor : EditorWindow
    {
        [MenuItem("AsunaClient/Scene/SceneEditor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            SceneEditor window = (SceneEditor)EditorWindow.GetWindow(typeof(SceneEditor));
            if (_InitRoot())
            {
                _CollectSceneData();
                window.Show();
            }
            else
            {
                XDebug.Error("Init root fail!");
            }
        }


        private static bool _InitRoot()
        {
            var root = GameObject.Find(_RootName);
            if (root != null)
            {
                return false;
            }
            _Root = new GameObject(_RootName);
            return true;
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

        private List<SceneItem> _CollectSceneItems()
        {
            return null;
        }

        private void _ExportSceneData()
        {
            var items = _CollectSceneItems();
            
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

        private void _Clear()
        {
            
        }

        private void OnDestroy()
        {
            if (_Root == null)
            {
                XDebug.Error("unknown error!");
                return;
            }

            _Clear();
            DestroyImmediate(_Root);
        }

        private static GameObject _Root;
        private static SceneData _CurrentScene;
        private const string _RootName = "SceneEditorRoot";
    }
}