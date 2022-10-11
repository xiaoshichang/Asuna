using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asuna.Foundation
{
    public static partial class UIManager
    {
        public static void Init()
        {
            _RegisterUIEntries();
            _InitHierarchy();
        }

        public static void Release()
        {
            _ReleaseHierarchy();
        }
        
    }

}

