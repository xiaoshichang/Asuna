using System.Collections;
using AsunaClient.Foundation;
using AsunaClient.Foundation.UI;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitUIManager()
        {
            UIManager.Init(null);
            XDebug.Info("Init UI Manager Ok!");
        }

        private void _ReleaseUIManager()
        {
            UIManager.Release();
            XDebug.Info("Release UI Manager Ok!");
        }

        public readonly UIManager UIManager = new UIManager();
    }
}