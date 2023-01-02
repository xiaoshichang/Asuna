using Codice.Utils.Buffers;
using UnityEditor;

namespace Asuna.Asset
{
    public static class AssetProviderModeSelect
    {
        private const string _CheckPath = "Asuna/Asset/AssetProviderMode";
        private const string _EditorModePath = "Asuna/Asset/AssetProviderMode/EditorMode";
        private const string _AssetBundlePath = "Asuna/Asset/AssetProviderMode/AssetBundleMode";
        

        [MenuItem(_CheckPath, true)]
        private static bool _Check()
        {
            var mode = EditorPrefs.GetString(IAssetProvider.ProviderMode_Key);
            if (string.IsNullOrEmpty(mode) || mode == IAssetProvider.ProviderMode_Value_Editor)
            {
                _EnableEditorMode();
            }
            else if (mode == IAssetProvider.ProviderMode_Value_AB)
            {
                _EnableAssetBundleMode();
            }

            return true;
        }
        
        [MenuItem(_EditorModePath)]
        private static void _EnableEditorMode()
        {
            Menu.SetChecked(_EditorModePath, true);
            Menu.SetChecked(_AssetBundlePath, false);
            EditorPrefs.SetString(IAssetProvider.ProviderMode_Key, IAssetProvider.ProviderMode_Value_Editor);
        }
        
        [MenuItem(_AssetBundlePath)]
        private static void _EnableAssetBundleMode()
        {
            Menu.SetChecked(_EditorModePath, false);
            Menu.SetChecked(_AssetBundlePath, true);
            EditorPrefs.SetString(IAssetProvider.ProviderMode_Key, IAssetProvider.ProviderMode_Value_AB);
        }
    }
}