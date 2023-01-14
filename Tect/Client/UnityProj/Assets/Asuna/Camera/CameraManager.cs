using System.Collections.Generic;
using System.Linq;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Camera
{
    public class CameraManager : IManager
    {
        public void Init(object param)
        {
            _Camera = GameObject.Find("Main Camera").GetComponent<UnityEngine.Camera>();
            ADebug.Assert(_Camera != null);
        }

        public void Release()
        {
        }

        public void Tick(float dt)
        {
            _Current?.Tick(dt);
        }

        public void PushCameraMode(CameraMode mode)
        {
            _Current = mode;
            _CameraModes.Add(mode);
            _Current.Enter();
        }

        public void PushFollowMode(Transform target, Vector3 eyeVector)
        {
            var mode = new FollowMode()
            {
                Target = target,
                EyeVector = eyeVector
            };
            PushCameraMode(mode);
        }

        public void PopCameraMode()
        {
            if (_CameraModes.Count <= 0)
            {
                ADebug.Warning("empty");
                return;
            }
            _Current = _CameraModes.Last();
            _Current.Leave();
            _CameraModes.Remove(_Current);
            _Current = _CameraModes.Last();
            _Current.Enter();
        }

        public UnityEngine.Camera GetCamera()
        {
            return _Camera;
        }

        private UnityEngine.Camera _Camera;
        private CameraMode _Current;
        private readonly List<CameraMode> _CameraModes = new List<CameraMode>();

    }
}