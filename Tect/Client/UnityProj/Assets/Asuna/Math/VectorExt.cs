using UnityEngine;

namespace Asuna.Math
{
    public static class VectorExt
    {
        public static Vector3 ToX0Z(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }
    }
}