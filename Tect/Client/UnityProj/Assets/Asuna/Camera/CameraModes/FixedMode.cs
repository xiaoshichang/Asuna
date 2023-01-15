
using Asuna.Application;
using UnityEngine;

namespace Asuna.Camera
{
    public class FixedMode : CameraMode
    {
        public override void Enter()
        {
            var camera = G.CameraManager.GetCamera();
            camera.transform.position = Target - EyeVector;
            camera.transform.forward = EyeVector;
        }

        public override void Tick(float dt)
        {
        }

        public override void Leave()
        {
        }
        
        public Vector3 Target;
        public Vector3 EyeVector;
    }
}