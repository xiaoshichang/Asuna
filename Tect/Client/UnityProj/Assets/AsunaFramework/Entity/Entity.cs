using System;
using UnityEngine;

namespace AF.Entity
{
    public class Entity
    {
        public Entity(Guid guid)
        {
            Guid = guid;
        }

        public Entity()
        {
            Guid = Guid.NewGuid();
        }

        public virtual void Destroy()
        {
            
        }
        
        public Guid Guid;
    }
}