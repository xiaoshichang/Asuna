using Asuna.Application;
using Asuna.Utils;

namespace Demo.UIBasic
{
    public class DemoCaseUIBasic : DemoCaseBase
    {
        public override void InitDemo()
        {
            ADebug.Info("Init DemoCaseUIBasic");
            G.UIManager.ShowPage(nameof(DemoCaseUIBasicPage), null);
        }

        public override void ReleaseDemo()
        {
            G.UIManager.HidePage(nameof(DemoCaseUIBasicPage));
        }

        public override void Tick(float dt)
        {
            
        }

        public override string GetBtnName()
        {
            return "UI Basic";
        }
    }
}