
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
    }
}