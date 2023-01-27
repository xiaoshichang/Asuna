using System.Collections;
using Asuna.Application;
using Asuna.Entity;
using Asuna.Input;
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
            G.CameraManager.GetMainCamera().PushFixedMode(new Vector3(10, 0, 10), new Vector3(0, -20, 25));
            G.UIManager.ScreenFadeTo(Color.black, 0.2f, _OnFadeFinish);
        }

        public override void ReleaseDemo()
        {
            G.UIManager.HidePage(nameof(DemoCaseLoadScenePage));
            G.EntityManager.DestroySpace(_Space);
            G.EntityManager.DestroyAvatar(_MainPlayer);
            G.CameraManager.GetMainCamera().PopCameraMode();
        }

        public override void Tick(float dt)
        {
        }

        public override string GetDemoName()
        {
            return "Load Scene";
        }

        private void _OnFadeFinish()
        {
            _Space = G.EntityManager.CreateSpaceEntity();
            var request = new LoadSceneRequest()
            {
                ScenePath = "Assets/Demo/Res/SceneData/Demo.asset",
                OnSceneLoaded = _OnSceneLoadFinish
            };
            _Space.LoadScene(request);
        }

        private void _OnSceneLoadFinish(LoadSceneRequest request)
        {
            G.UIManager.ShowPage(nameof(DemoCaseLoadScenePage), null);
            G.CoroutineManager.StartGlobalCoroutine(_LoadPlayer());
        }

        private IEnumerator _LoadPlayer()
        {
            var avatarInitParam = new AvatarInitParam
            {
                ModelComponentInitParam =
                {
                    ModelAssetPath = "Assets/Demo/Res/Character/Capsule.prefab"
                },
                CharacterControllerComponentInitParam =
                {
                    ControllerMode = ControllerMode.NativeAnimator
                },
                PlayerInputComponentInitParam =
                {
                    PlayerInputCandidate = true
                }
            };

            _MainPlayer = G.EntityManager.CreateAvatarEntity(avatarInitParam);
            yield return _MainPlayer.LoadAsync();
            _MainPlayer.SetPosition(new Vector3(2, 0, -2));
            G.UIManager.ClearFade(0.2f);
        }

        protected SpaceEntity _Space;
        protected AvatarEntity _MainPlayer;

    }
}