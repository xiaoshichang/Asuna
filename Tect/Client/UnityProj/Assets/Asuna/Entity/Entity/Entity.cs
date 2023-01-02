using System;
using System.Collections;
using Asuna.Application;
using Asuna.Entity.Mono;
using Asuna.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.Entity
{
    public abstract class Entity
    {
        protected abstract string GetGameObjectName();

        #region Constructor
        protected Entity(Guid guid)
        {
            Guid = guid;
        }

        protected Entity()
        {
            Guid = Guid.NewGuid();
        }
        #endregion

        #region Coroutine
        private void _InitCoroutineManager()
        {
            _CoroutineManager = _Root.AddComponent<CoroutineManager>();
        }
        protected CoroutineManager _CoroutineManager;
        #endregion
       
        public virtual void Init()
        {
            // name
            _Root = new GameObject(GetGameObjectName());
            // parent
            _Root.transform.SetParent(G.Application.EntityManager.GetRoot().transform);

            _InitCoroutineManager();
        }

        public virtual void Destroy()
        {
            ADebug.Assert(_Root != null);
            Object.Destroy(_Root);
        }

        
        public Guid Guid;
        protected GameObject _Root;

    }
}