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
            ADebug.Info("Init DemoCaseLoadScene");
            _StartScreenFade();
        }

        public override void ReleaseDemo()
        {
            G.CameraManager.PopCameraMode();
            G.UIManager.HidePage(nameof(DemoCaseLoadScenePage));
            G.EntityManager.DestroySpace(_Space);
            G.EntityManager.DestroyAvatar(_Player);
            G.PlayerInputManager.RemovePlayerInputMapping(PlayerType.Player1);
        }

        public override void Tick(float dt)
        {
            if (_ActionSet != null)
            {
                if (_ActionSet.Fire.IsPressed)
                {
                    ADebug.Info($"fire is pressed!");
                }

                if (_ActionSet.Jump.IsPressed)
                {
                    ADebug.Info("jump is pressed!");
                }
            }
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

        private void _InitCamera()
        {
            G.CameraManager.PushFixedMode(new Vector3(10, 0, 10), new Vector3(0, -20, 25));
        }

        private void _InitPlayerInput()
        {
            var device = G.PlayerInputManager.GetAvailableDevice();
            if (device == null)
            {
                ADebug.Warning("no available device found!");
                return;
            }
            var actionSet = new DefaultPlayerActionSet();
            var mapping = new PlayerInputMapping(device, actionSet);
            G.PlayerInputManager.SetupPlayerInputMapping(PlayerType.Player1, mapping);

            _ActionSet = mapping.GetActionSet() as DefaultPlayerActionSet;
        }

        private IEnumerator _LoadPlayer()
        {
            var avatarData = new AvatarData()
            {
                ModelAsset = "Assets/Demo/Res/Character/Capsule.prefab"
            };
            
            _Player = G.EntityManager.CreateAvatarEntity(avatarData);
            _InitPlayerInput();
            _InitCamera();
            yield return _Player.LoadAsync();
            _Player.SetPosition(new Vector3(2, 0, -2));
            G.UIManager.ClearFade(0.2f);
        }

        private DefaultPlayerActionSet _ActionSet;
        private SpaceEntity _Space;
        private AvatarEntity _Player;

    }
}