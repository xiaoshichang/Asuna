using System.Collections;
using Asuna.Application;
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
            G.EntityManager.DestroyAvatar(_Player);
        }

        public override string GetBtnName()
        {
            return "Load Scene";
        }

        private void _StartScreenFade()
        {
            G.UIManager.ScreenFadeTo(Color.black, 0.2f, _OnFadeFinish);
        }

        private void _OnFadeFinish()
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
            G.CoroutineManager.StartGlobalCoroutine(_LoadPlayer());
        }

        private IEnumerator _LoadPlayer()
        {
            var avatarData = new AvatarData()
            {
                ModelAsset = "Assets/Demo/Res/Character/Capsule.prefab"
            };
            
            _Player = G.EntityManager.CreateAvatarEntity(avatarData);
            yield return _Player.LoadAsync();
            _Player.SetPosition(new Vector3(2, 0, -2));
            G.UIManager.ClearFade(0.2f);
        }

        private SpaceEntity _Space;
        private AvatarEntity _Player;

    }
}