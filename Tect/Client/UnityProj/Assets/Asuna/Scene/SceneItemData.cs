using System;
using UnityEngine;

namespace Asuna.Scene
{
    [Serializable]
    public struct SavedVector3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public static SavedVector3 FromVector3(Vector3 v)
        {
            var sv = new SavedVector3
            {
                x = v.x,
                y = v.y,
                z = v.z
            };
            return sv;
        }
    }

    [Serializable]
    public struct SavedQuaternion
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }

        public static SavedQuaternion FromQuaternion(Quaternion q)
        {
            var sq = new SavedQuaternion
            {
                x = q.x,
                y = q.y,
                z = q.z,
                w = q.w
            };
            return sq;
        }
    }
    
    
    /// <summary>
    /// SceneItem 的持久化数据格式
    /// </summary>
    [Serializable]
    public class SceneItemData
    {
        public SavedVector3 P { get; set; }
        public SavedQuaternion R { get; set; }
        public SavedVector3 S { get; set; }
        public string Asset { get; set; }
        public string Name { get; set; }

        
    }
}