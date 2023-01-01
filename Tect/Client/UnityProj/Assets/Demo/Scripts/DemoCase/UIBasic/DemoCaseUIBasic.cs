using Asuna.Application;
using Asuna.Utils;

namespace Demo.UIBasic
{
    public class DemoCaseUIBasic : DemoCaseBase
    {
        public override void InitDemo()
        {
            XDebug.Info("Init DemoCaseUIBasic");
            G.UIManager.ShowPage(nameof(DemoCaseUIBasicPage), null);
        }

        public override void ReleaseDemo()
        {
            G.UIManager.HidePage(nameof(DemoCaseUIBasicPage));
        }

        public override string GetBtnName()
        {
            return "UI Basic";
        }
    }
}