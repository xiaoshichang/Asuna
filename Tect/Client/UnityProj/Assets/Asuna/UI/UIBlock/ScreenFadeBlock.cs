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
            gameObject.SetActive(false);
            _Image = GetComponent<Image>();
            _Image.color = Color.clear;
            _Image.raycastTarget = true;
        }

        private void Update()
        {
            if (_Process < 0)
            {
                return;
            }
            
            _Process += Time.deltaTime / _Interval;
            _Image.color = Color.Lerp(_SourceColor, _TargetColor, _Process);
            
            if (_Process >= 1)
            {
                if (_Clearing)
                {
                    gameObject.SetActive(false);
                }
               
                _OnFadeFinishCallback?.Invoke();
                _OnFadeFinishCallback = null;
                _Process = -1;
            }
        }

        public void FadeTo(Color color, float intervalIsSecond=1, Action OnFadeFinish=null)
        {
            ADebug.Assert(_OnFadeFinishCallback == null);
            _OnFadeFinishCallback = OnFadeFinish;
            
            gameObject.SetActive(true);
            _SourceColor = _Image.color;
            _TargetColor = color;
            _Interval = intervalIsSecond;
            _Process = 0;
            _Clearing = false;
        }

        public void Clear(float interval=1, Action OnFadeFinish=null)
        {
            ADebug.Assert(_OnFadeFinishCallback == null);
            _OnFadeFinishCallback = OnFadeFinish;
            
            _SourceColor = _Image.color;
            _TargetColor = Color.clear;
            _Interval = interval;
            _Process = 0;
            _Clearing = true;
        }

        private Image _Image;
        private Color _TargetColor;
        private Color _SourceColor;
        private float _Interval;
        private bool _Clearing;
        
        /// <summary>
        /// 0 - 1 代表渐变过程的开始和结束
        /// -1 代表静止过程
        /// </summary>
        private float _Process = -1;

        /// <summary>
        /// fade 完成的回调
        /// </summary>
        private Action _OnFadeFinishCallback;

    }
}