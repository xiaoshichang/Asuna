using System.Collections;
using Asuna.Application;
using Asuna.Interface;

namespace Asuna.Coroutine
{
    public class CoroutineManager : IManager
    {

        public void Init(object param)
        {
            _Root = param as ApplicationRoot;
        }

        public void Release()
        {
            _Root.StopAllCoroutines();
            _Root = null;
        }
        
        public UnityEngine.Coroutine StartGlobalCoroutine(IEnumerator routine)
        {
            return _Root.StartCoroutine(routine);
        }

        public void StopGlobalCoroutine(UnityEngine.Coroutine routine)
        {
            _Root.StopCoroutine(routine);
        }

        public void StopGlobalCoroutine(IEnumerator routine)
        {
            _Root.StopCoroutine(routine);
        }

        public void StopAllGlobalCoroutines()
        {
            _Root.StopAllCoroutines();
        }

        private ApplicationRoot _Root;

    }
}