using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsunaClient.Foundation
{
    public static partial class UIManager
    {
        public static void Init()
        {
            _RegisterPages();
            _InitHierarchy();
        }

        public static void Release()
        {
            _ReleaseHierarchy();
        }
        
    }

}

