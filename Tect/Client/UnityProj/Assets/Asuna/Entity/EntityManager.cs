using System;
using System.Collections.Generic;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.Entity
{
    public class EntityManager : IManager
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
            XDebug.Assert(_Root != null);
            XDebug.Assert(_Entities != null);
            XDebug.Assert(_Entities.Count == 0);
            Object.DestroyImmediate(_Root);
        }

        private GameObject _Root;
        private readonly Dictionary<Guid, Entity> _Entities = new Dictionary<Guid, Entity>();
    }
}