using Asuna.Utils;

namespace Demo.UIBasic
{
    public class DemoCaseUIBasic : DemoCaseBase
    {
        public override void InitDemo()
        {
            XDebug.Info("Init DemoCaseUIBasic");
        }

        public override void ReleaseDemo()
        {
        }

        public override string GetBtnName()
        {
            return "UI Basic";
        }
    }
}