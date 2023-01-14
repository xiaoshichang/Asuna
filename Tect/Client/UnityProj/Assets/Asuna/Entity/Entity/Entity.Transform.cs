
using UnityEngine;

namespace Asuna.Entity
{
    public abstract partial class Entity
    {
        public void SetPosition(Vector3 position)
        {
            _Root.transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return _Root.transform.position;
        }

        public void SetRotation(Quaternion rotation)
        {
            _Root.transform.rotation = rotation;
        }

        public Quaternion GetRotation()
        {
            return _Root.transform.rotation;
        }

        public void SetScale(Vector3 scale)
        {
            _Root.transform.localScale = scale;
        }

        public Vector3 GetScale()
        {
            return _Root.transform.localScale;
        }

        public Transform GetTransform()
        {
            return _Root.transform;
        }
    }
}