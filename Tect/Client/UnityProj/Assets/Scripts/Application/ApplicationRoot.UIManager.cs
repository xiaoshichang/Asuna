using System.Collections;
using AsunaClient.Foundation;
using AsunaClient.Foundation.UI;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitUIManager()
        {
            UIManager.Init();
            XDebug.Info("Init UI Ok!");
        }

        private void _ReleaseUIManager()
        {
            UIManager.Release();
        }

        public readonly UIManager UIManager = new UIManager();
    }
}