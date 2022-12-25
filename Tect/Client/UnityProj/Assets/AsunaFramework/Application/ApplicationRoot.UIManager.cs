using System.Collections;
using AF.Utils;
using AF.UI;

namespace AF.Application
{
    public partial class ApplicationRoot
    {
        private void _InitUIManager()
        {
            UIManager = new UIManager();
            var param = new UIManagerInitParam()
            {
                AssetManager = AssetManager,
                PageRegisterItems = ApplicationSetting.UIPageRegisterItems
            };
            UIManager.Init(param);
            XDebug.Info("Init UI Manager Ok!");
        }

        private void _ReleaseUIManager()
        {
            UIManager.Release();
            UIManager = null;
            XDebug.Info("Release UI Manager Ok!");
        }

        public UIManager UIManager;
    }
}