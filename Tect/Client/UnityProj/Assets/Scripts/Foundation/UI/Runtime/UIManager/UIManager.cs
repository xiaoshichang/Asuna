using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsunaClient.Foundation.UI
{
    public partial class UIManager
    {
        public void Init()
        {
            _RegisterPages();
            _InitHierarchy();
        }

        public void Release()
        {
            _ReleaseHierarchy();
        }
        
    }

}

