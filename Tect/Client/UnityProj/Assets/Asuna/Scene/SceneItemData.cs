using System;
using UnityEngine;

namespace Asuna.Scene
{
    
    /// <summary>
    /// SceneItem 的持久化数据格式
    /// </summary>
    [Serializable]
    public class SceneItemData
    {
        public Vector3 P;
        public Vector3 S;
        public Quaternion R;
        public string Asset;
        public string Name;

    }
}