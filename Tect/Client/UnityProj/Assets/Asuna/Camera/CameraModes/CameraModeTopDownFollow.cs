using Asuna.Application;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Camera.CameraModes
{
    public class CameraModeTopDownFollow : CameraMode
    {
        public override void Enter()
        {
            ADebug.Assert(_Camera is null);
            _Camera = G.CameraManager.GetCamera();
            _CameraTransform = _Camera.transform;
        }

        public override void Tick(float dt)
        {
            if (Target is null)
            {
                return;
            }
            
            _CameraTransform.position = Target.position - EyeVector;
            _CameraTransform.forward = EyeVector;
        }

        public override void Leave()
        {
            ADebug.Assert(_Camera is not null);
            Target = null;
            _Camera = null;
            _CameraTransform = null;
        }

        private UnityEngine.Camera _Camera;
        private Transform _CameraTransform;

        public Transform Target;
        public Vector3 EyeVector;
    }
}