using System.Collections;
using UnityEngine;

namespace Asuna.Entity.Mono
{
    public class CoroutineManager : MonoBehaviour
    {
        public void StartCoroutine_(IEnumerator routine)
        {
            StartCoroutine(routine);
        }

        public void StopCoroutine_(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        public void StopAllCoroutines_()
        {
            StopAllCoroutines();
        }
        
    }
}