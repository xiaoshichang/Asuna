﻿using System;
using UnityEngine;

namespace Asuna.Entity
{
    public class Entity
    {
        public Entity(Guid guid)
        {
            Guid = guid;
        }

        protected Entity()
        {
            Guid = Guid.NewGuid();
        }

        public virtual void Destroy()
        {
            
        }
        
        public Guid Guid;
    }
}