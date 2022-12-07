using System;
using UnityEngine;

namespace AsunaClient.Application.Entity
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
        
        public Guid Guid;
    }
}