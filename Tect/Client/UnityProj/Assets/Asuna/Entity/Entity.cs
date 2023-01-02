using System;
using UnityEngine;

namespace Asuna.Entity
{
    public abstract class Entity
    {
        protected Entity(Guid guid)
        {
            Guid = guid;
        }

        protected Entity()
        {
            Guid = Guid.NewGuid();
        }

        public abstract void Init();
        public abstract void Release();
        
        public Guid Guid;
    }
}