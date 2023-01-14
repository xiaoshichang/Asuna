using System;
using Asuna.Application;
using Asuna.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

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
            _Root = new GameObject(GetGameObjectName());
            // parent
            _Root.transform.SetParent(G.Application.EntityManager.GetRoot().transform);

        }

        public virtual void Destroy()
        {
            ADebug.Assert(_Root != null);
            Object.Destroy(_Root);
        }

        public GameObject GetRoot()
        {
            return _Root;
        }

        
        public Guid Guid;
        protected GameObject _Root;

    }
}