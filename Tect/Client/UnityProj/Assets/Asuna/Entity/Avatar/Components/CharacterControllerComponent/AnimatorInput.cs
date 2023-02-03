using UnityEngine;

namespace Asuna.Entity
{

    
    
    public class AnimatorInput
    {
        /// <summary>
        /// 移动方向
        /// </summary>
        public Vector3 NormalizeMoveDirection;

        /// <summary>
        /// 移动中标记
        /// </summary>
        public bool IsMoving;


        public void Reset()
        {
            IsMoving = false;
            NormalizeMoveDirection = Vector3.zero;
        }
    }
}