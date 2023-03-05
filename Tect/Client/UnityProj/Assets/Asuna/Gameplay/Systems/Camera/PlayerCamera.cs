using System.Collections.Generic;
using System.Linq;
using Asuna.Foundation.Debug;
using UnityEngine;

namespace Asuna.Camera
{
    public class PlayerCamera
    {
        public void Init(UnityEngine.Camera camera)
        {
            _Camera = camera;
        }

        public void Release()
        {
            _CameraModes.Clear();
        }

        public void Tick(float dt)
        {
            _Current?.Tick(dt);
        }

        public UnityEngine.Camera GetRawCamera()
        {
            return _Camera;
        }
        
        private void _PushCameraMode(CameraMode mode)
        {
            _Current = mode;
            _CameraModes.Add(mode);
            _Current.Enter();
        }

        public void PushFollowMode(Transform target, Vector3 eyeVector)
        {
            var mode = new FollowMode(_Camera)
            {
                Target = target,
                EyeVector = eyeVector
            };
            _PushCameraMode(mode);
        }

        public void PushFixedMode(Vector3 target, Vector3 eyeVector)
        {
            var mode = new FixedMode(_Camera)
            {
                Target = target,
                EyeVector = eyeVector
            };
            _PushCameraMode(mode);
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

            if (_CameraModes.Count > 0)
            {
                _Current = _CameraModes.Last();
                _Current.Enter();
            }
        }
        
        private UnityEngine.Camera _Camera;
        private CameraMode _Current;
        private readonly List<CameraMode> _CameraModes = new();
    }
}