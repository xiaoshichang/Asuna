using UnityEditor;
using UnityEngine;

namespace Asuna.Scene.Editor
{
    public partial class SceneEditor
    {

        #region Current Scene Dropdown

        private void _RenderCurrentSceneDropdown()
        {
            _CurrentSceneIdx = EditorGUILayout.Popup("Current Editing Scene", _CurrentSceneIdx, _AllScenes);
        }

        private string _GetCurrentSelectedScenePath()
        {
            return _AllScenes[_CurrentSceneIdx];
        }
        
        private static string[] _AllScenes;
        private int _CurrentSceneIdx = 0;

        #endregion

        #region Load Scene

        private void _RenderLoadScene()
        {
            if (GUILayout.Button("Load Scene Data"))
            {
                _LoadScene();
            }
        }
        #endregion

        #region Export Scene
        private void _RenderExportScene()
        {
            if (GUILayout.Button("Export Scene Data"))
            {
                _ExportScene();
            }
        }
        #endregion
        
        void OnGUI()
        {
            _RenderCurrentSceneDropdown();
            _RenderLoadScene();
            _RenderExportScene();
        }
        
        

    }
}