using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsunaClient.Foundation.UI
{
    public class ShowPageParam
    {
    }
    
    public abstract class UIPage
    {
        public abstract string GetAssetPath();
        public abstract void OnShow(ShowPageParam param);
        public abstract void OnHide();
        public abstract void BeforeDestroy();
    }

}
