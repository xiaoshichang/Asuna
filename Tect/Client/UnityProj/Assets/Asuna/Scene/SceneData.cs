using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asuna.Scene
{
    /// <summary>
    /// 场景的持久化数据格式
    /// </summary>
    [CreateAssetMenu(fileName = "Default", menuName = "Asuna/Scene/SceneData", order = 1)]
    public class SceneData : ScriptableObject
    {
        public List<SceneItemData> SceneItems = new List<SceneItemData>();
    }
    
}