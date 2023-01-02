﻿using Asuna.Application;
using Asuna.Entity;
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
            ADebug.Info("Init DemoCaseLoadScene");
            _StartScreenFade();
        }

        public override void ReleaseDemo()
        {
            G.UIManager.HidePage(nameof(DemoCaseLoadScenePage));
            G.EntityManager.DestroySpace(_Space);
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
            _Space = G.EntityManager.CreateSpaceEntity();
            var request = new LoadSceneRequest()
            {
                ScenePath = "Assets/Demo/Res/SceneData/Demo.asset",
                OnSceneLoaded = _OnLoadFinish
            };
            _Space.LoadScene(request);
        }

        private void _OnLoadFinish(LoadSceneRequest request)
        {
            G.UIManager.ShowPage(nameof(DemoCaseLoadScenePage), null);
            G.UIManager.ClearFade(0.2f);
        }

        private SpaceEntity _Space;

    }
}