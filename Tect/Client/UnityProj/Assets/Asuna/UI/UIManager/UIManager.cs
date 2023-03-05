using System.Collections.Generic;
using Asuna.Foundation.Debug;
using Asuna.Foundation.Interface;

namespace Asuna.UI
{
    public class UIManagerInitParam
    {
        public List<UIPageRegisterItem> PageRegisterItems;
    }
    
    
    public partial class UIManager : IManager
    {
        public void Init(object param)
        {
            var initParam = param as UIManagerInitParam;
            ADebug.Assert(initParam != null);
            
            _RegisterPages(initParam.PageRegisterItems);
            _InitHierarchy();
            _InitScreenFadeBlock();
        }

        public void Release()
        {
            _ReleaseScreenFadeBlock();
            _ReleaseStack();
            _UnRegisterPages();
            _ReleaseHierarchy();
        }
    }

}

