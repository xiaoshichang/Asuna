using Asuna.Application;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Camera
{
    public class FollowMode : CameraMode
    {
        public FollowMode(UnityEngine.Camera camera) : base(camera)
        {
        }
        
        public override void Enter()
        {
            ADebug.Assert(_Camera is null);
        }

        public override void Tick(float dt)
        {
            if (Target is null)
            {
                return;
            }

            var transform = _Camera.transform;
            transform.position = Target.position - EyeVector;
            transform.forward = EyeVector;
        }

        public override void Leave()
        {
            ADebug.Assert(_Camera is not null);
            Target = null;
        }


        public Transform Target;
        public Vector3 EyeVector;

        
    }
}