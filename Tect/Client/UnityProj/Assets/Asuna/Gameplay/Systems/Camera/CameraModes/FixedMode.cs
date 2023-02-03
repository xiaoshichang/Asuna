using UnityEngine;

namespace Asuna.Camera
{
    public class FixedMode : CameraMode
    {
        public FixedMode(UnityEngine.Camera camera) : base(camera)
        {
        }
        
        public override void Enter()
        {
            var transform = _Camera.transform;
            transform.position = Target - EyeVector;
            transform.forward = EyeVector;
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