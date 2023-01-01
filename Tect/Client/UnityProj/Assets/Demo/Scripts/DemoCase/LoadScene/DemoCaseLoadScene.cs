using Asuna.Application;
using Asuna.Timer;
using Asuna.Utils;
using Demo.UIBasic;
using UnityEngine;

namespace Demo.LoadScene
{
    public class DemoCaseLoadScene : DemoCaseBase
    {
        public override void InitDemo()
        {
            XDebug.Info("Init DemoCaseLoadScene");
            _StartScreenFade();
        }

        public override void ReleaseDemo()
        {
            G.UIManager.HidePage(nameof(DemoCaseLoadScenePage));
        }

        public override string GetBtnName()
        {
            return "Load Scene";
        }

        private void _StartScreenFade()
        {
            G.UIManager.ScreenFadeTo(Color.black, 0.2f);
            G.TimerManager.RegisterTimer(200, _OnFadeFinish);
            
        }

        private void _OnFadeFinish(object arg)
        {
            
        }

        private void _OnLoadFinish()
        {
            G.UIManager.ShowPage(nameof(DemoCaseLoadScenePage), null);
        }
        
    }
}