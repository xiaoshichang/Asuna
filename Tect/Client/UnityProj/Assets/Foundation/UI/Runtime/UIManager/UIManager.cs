using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation.Interface;
using UnityEngine;

namespace AsunaClient.Foundation.UI
{
    public partial class UIManager : IManager
    {
        public void Init(object param)
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

