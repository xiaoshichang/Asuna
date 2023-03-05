﻿using Asuna.Asset;
using Asuna.Foundation.Debug;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        private void _InitAssetManager()
        {
            AssetManager = new AssetManager();
            AssetManager.Init(null);
            ADebug.Info("Init Asset Manager Ok!");
        }

        private void _ReleaseAssetManager()
        {
            AssetManager.Release();
            AssetManager = null;
            ADebug.Info("Release Asset Manager Ok!");
        }

        public AssetManager AssetManager;
    }
}