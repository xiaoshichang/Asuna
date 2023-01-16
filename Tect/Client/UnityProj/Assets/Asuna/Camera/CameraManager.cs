using System.Collections.Generic;
using System.Linq;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Camera
{
    public class CameraManager : IManager
    {
        private void _InitMainCamera()
        {
            var mainCamera = GameObject.Find("Cameras/Main Camera").GetComponent<UnityEngine.Camera>();
            var runningCamera = new RunningCamera();
            runningCamera.Init(mainCamera);
            _RunningCameras.Add(runningCamera);
        }
        
        public void Init(object param)
        {
            _InitMainCamera();
            ADebug.Assert(_RunningCameras.Count == 1);
        }

        public void Release()
        {
            foreach (var runningCamera in _RunningCameras)
            {
                runningCamera.Release();
            }
            _RunningCameras.Clear();
        }

        public void LateUpdate(float dt)
        {
            foreach (var runningCamera in _RunningCameras)
            {
                runningCamera.Tick(dt);
            }
        }

        public RunningCamera GetMainCamera()
        {
            return GetRunningCamera(0);
        }

        public RunningCamera GetRunningCamera(int index)
        {
            if (_RunningCameras.Count <= index)
            {
                return null;
            }
            return _RunningCameras[index];
        }

        private readonly List<RunningCamera> _RunningCameras = new();

    }
}