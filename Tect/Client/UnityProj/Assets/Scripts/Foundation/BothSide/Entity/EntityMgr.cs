
using System;
using System.Collections.Generic;


namespace Asuna.Foundation
{
    public static class EntityMgr
    {
        public static void Init()
        {
        }

        public static Entity Create(Type t)
        {
            if (Activator.CreateInstance(t) is not Entity e)
            {
                throw new InvalidCastException();
            }
            _Entities.Add(e);
            return e;
        }

        private static readonly List<Entity> _Entities = new();

    }
}
