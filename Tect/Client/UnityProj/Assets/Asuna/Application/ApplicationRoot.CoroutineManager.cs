using System.Collections;
using Asuna.Coroutine;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        private void _InitCoroutineManager()
        {
            CoroutineManager = new CoroutineManager();
            CoroutineManager.Init(this);
        }

        private void _ReleaseCoroutineManager()
        {
            CoroutineManager.Release();
            CoroutineManager = null;
        }

        public CoroutineManager CoroutineManager;
    }
}