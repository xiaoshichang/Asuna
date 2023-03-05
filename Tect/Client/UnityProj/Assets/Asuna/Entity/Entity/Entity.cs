using System;
using Asuna.Application;
using Asuna.Foundation.Debug;
using UnityEngine;


namespace Asuna.Entity
{
    public abstract partial class Entity
    {
        /// <summary>
        /// 显示在Unity GameObject 上的名称
        /// </summary>
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

        
        public virtual void Init(object param)
        {
            // name
            _RootGO = new GameObject(GetGameObjectName());
            // parent
            _RootGO.transform.SetParent(G.Application.EntityManager.GetRoot().transform);

        }

        public virtual void Destroy()
        {
            ADebug.Assert(_RootGO is not null);
            UnityEngine.Object.Destroy(_RootGO);
        }

        public abstract void Update(float dt);
        public abstract void LateUpdate(float dt);

        public GameObject GetRootGO()
        {
            return _RootGO;
        }

        
        public Guid Guid;
        protected GameObject _RootGO;

    }
}