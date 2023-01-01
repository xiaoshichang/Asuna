using Asuna.Utils;

namespace Demo.LoadScene
{
    public class DemoCaseLoadScene : DemoCaseBase
    {
        public override void InitDemo()
        {
            XDebug.Info("Init DemoCaseLoadScene");
        }

        public override void ReleaseDemo()
        {
        }

        public override string GetBtnName()
        {
            return "Load Scene";
        }
    }
}