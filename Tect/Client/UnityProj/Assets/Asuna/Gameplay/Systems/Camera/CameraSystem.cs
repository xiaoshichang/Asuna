using System.Collections.Generic;
using System.Linq;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Camera
{
    public class CameraSystem : ISystem
    {
        private void _InitMainCamera()
        {
            var mainCamera = GameObject.Find("Cameras/Main Camera").GetComponent<UnityEngine.Camera>();
            var playerCamera = new PlayerCamera();
            playerCamera.Init(mainCamera);
            _PlayerCameras.Add(playerCamera);
        }
        
        public void Init(object param)
        {
            _InitMainCamera();
            ADebug.Assert(_PlayerCameras.Count == 1);
        }

        public void Release()
        {
            foreach (var runningCamera in _PlayerCameras)
            {
                runningCamera.Release();
            }
            _PlayerCameras.Clear();
        }

        public void LateUpdate(float dt)
        {
            foreach (var runningCamera in _PlayerCameras)
            {
                runningCamera.Tick(dt);
            }
        }

        public PlayerCamera GetMainCamera()
        {
            return GetPlayerCamera(0);
        }

        /// <summary>
        /// Get PlayerCamera by player index.
        /// </summary>
        public PlayerCamera GetPlayerCamera(int index)
        {
            if (_PlayerCameras.Count <= index)
            {
                return null;
            }
            return _PlayerCameras[index];
        }

        private readonly List<PlayerCamera> _PlayerCameras = new();

    }
}