
using UnityEngine;

namespace Asuna.Entity
{
    public abstract partial class Entity
    {
        public void SetPosition(Vector3 position)
        {
            _RootGO.transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return _RootGO.transform.position;
        }

        public void SetRotation(Quaternion rotation)
        {
            _RootGO.transform.rotation = rotation;
        }

        public Quaternion GetRotation()
        {
            return _RootGO.transform.rotation;
        }

        public void SetScale(Vector3 scale)
        {
            _RootGO.transform.localScale = scale;
        }

        public Vector3 GetScale()
        {
            return _RootGO.transform.localScale;
        }

        public Transform GetTransform()
        {
            return _RootGO.transform;
        }
    }
}