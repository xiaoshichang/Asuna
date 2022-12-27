using System;
using Asuna.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Asuna.UI
{

    [RequireComponent(typeof(Image))]
    public class ScreenFadeBlock : MonoBehaviour
    {
        public void Awake()
        {
            _Image = GetComponent<Image>();
            _Image.color = Color.clear;
            _Image.raycastTarget = true;
        }

        private void Update()
        {
            _Process += Time.deltaTime / _Interval;
            _Image.color = Color.Lerp(_SourceColor, _TargetColor, _Process);
            if (_Image.color.a <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="interval"> fade process will finish in interval seconds </param>
        public void FadeTo(Color color, float interval = 1)
        {
            gameObject.SetActive(true);
            _SourceColor = _Image.color;
            _TargetColor = color;
            _Interval = interval;
            _Process = 0;
        }

        public void Clear(float interval = 1)
        {
            _SourceColor = _Image.color;
            _TargetColor = Color.clear;
            _Interval = interval;
            _Process = 0;
        }

        private Image _Image;
        private Color _TargetColor;
        private Color _SourceColor;
        private float _Interval;
        
        /// <summary>
        /// 0 - 1 代表渐变过程的开始和结束
        /// </summary>
        private float _Process = 0;

    }
}