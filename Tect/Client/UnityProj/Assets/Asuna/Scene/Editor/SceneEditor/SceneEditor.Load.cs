using Asuna.Utils;
using Newtonsoft.Json;
using UnityEditor;

namespace Asuna.Scene.Editor
{
    public partial class SceneEditor
    {
        private void _LoadScene()
        {
            var path = EditorUtility.OpenFilePanel("Select a SceneData", "Assets/Demo/Res/SceneData", "SceneData");
            var content = FileUtils.ReadContentFromFileSync(path);
            var sceneData = JsonConvert.DeserializeObject<SceneData>(content);
            _InitScene();
            _RecoverSceneDataToEditor(sceneData);
        }

        
        private void _RecoverSceneDataToEditor(SceneData sceneData)
        {
            foreach (var itemData in sceneData.Items)
            {
                var item = SceneItemHelper.LoadFromSceneItemDataSync(itemData);
            }
        }
    }
}