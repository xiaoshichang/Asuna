using System;
using System.Collections.Generic;
using Asuna.Application;
using Asuna.Gameplay;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.Entity
{
    public partial class EntityManager : IManager
    {
        public void Init(object param)
        {
            _InitRootNode();
        }

        private void _InitRootNode()
        {
            _Root = new GameObject("EntityRoot");
            Object.DontDestroyOnLoad(_Root);
        }

        public void Release()
        {
            DestroyAllAvatars();
            DestroyAllSpace();
            
            ADebug.Assert(_Root != null);
            Object.DestroyImmediate(_Root);
        }

        public GameObject GetRoot()
        {
            return _Root;
        }

        [GM("entity.debug", "print entity manager debug info")]
        public static void DebugInfo()
        {
            G.Application.EntityManager._DebugInfo();
        }

        private void _DebugInfo()
        {
            string info = "entity count debug info\n";
            info += $"entity count: {_Entities.Count}\n";
            info += $"space count: {_SpaceEntities.Count}\n";
            info += $"avatar count: {_AvatarEntities.Count}\n";
            ADebug.Info(info);
        }

        private GameObject _Root;
        private readonly Dictionary<Guid, Entity> _Entities = new Dictionary<Guid, Entity>();
    }
}