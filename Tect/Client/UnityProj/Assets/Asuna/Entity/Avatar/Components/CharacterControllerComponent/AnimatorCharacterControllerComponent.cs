using System;
using Asuna.Application;
using Asuna.Input;
using Asuna.Math;
using Asuna.Utils;
using InControl;
using UnityEngine;

namespace Asuna.Entity
{
    /// <summary>
    /// Character Controller Based on Native Unity Animator
    /// </summary>
    public class AnimatorCharacterControllerComponent : CharacterControllerComponent
    {
        #region Collect Input Data

        private void _FillMovement(PlayerActionSet playerActionSet)
        {
            var defaultPlayerActionSet = playerActionSet as DefaultPlayerActionSet;
            if (defaultPlayerActionSet == null)
            {
                ADebug.Error("action set mismatch");
                return;
            }
            Vector3 direction = new Vector3(defaultPlayerActionSet.Move.X, 0, -defaultPlayerActionSet.Move.Y);
            if (direction.magnitude < 0.01f)
            {
                _InputData.IsMoving = false;
                _InputData.NormalizeMoveDirection = Vector3.zero;
            }
            else
            {
                _InputData.IsMoving = true;
                _InputData.NormalizeMoveDirection = direction;
            }
        }

        private void _FillRotation(PlayerActionSet playerActionSet)
        {
        }
        
        private void _CollectAnimatorInputFromPlayerInput()
        {
            ADebug.Assert(_CurrentBindPlayerType != PlayerType.None);
            
            var playerInputMapping = G.Application.PlayerInputManager.GetMapping(_CurrentBindPlayerType);
            if (playerInputMapping == null)
            {
                ADebug.Error("mapping not found ");
                return;
            }
            var playerActionSet = playerInputMapping.GetActionSet();
            if (playerActionSet == null)
            {
                ADebug.Error("unknown error");
                return;
            }

            _FillMovement(playerActionSet);
            _FillRotation(playerActionSet);

        }

        private readonly AnimatorInput _InputData = new ();
        #endregion

        #region Native Animator
        
        private void _FlushAnimatorInput(float dt)
        {
            
        }
        
        #endregion

        #region Apply Motion

        private void _ApplyMovement(float dt)
        {
            if (_InputData.IsMoving)
            {
                var delta = _InputData.NormalizeMoveDirection * (_Speed * dt);
                var position = _Owner.GetPosition();
                _Owner.SetPosition(position + delta);
            }
        }

        private void _ApplyRotation(float dt)
        {
            if (_InputData.IsMoving)
            {
                // 当前的yaw
                var curYawAngle = _Owner.GetRotation().eulerAngles.y;
                // 目标的yaw
                var targetYawAngle = Quaternion.LookRotation(_InputData.NormalizeMoveDirection.ToX0Z(), Vector3.up).eulerAngles.y;
                // 差值可正负，绝对值小于180
                var diff = Mathf.DeltaAngle(curYawAngle, targetYawAngle);
                var t = (dt * _AngelSpeed) / Mathf.Abs(diff);
                var expectedYawAngle = Mathf.LerpAngle(curYawAngle, targetYawAngle, t);
                var expectedQuaternion = Quaternion.Euler(0, expectedYawAngle, 0);
                _Owner.SetRotation(expectedQuaternion);
            }
        }

        private void _ApplyMotion(float dt)
        {
            _ApplyMovement(dt);
            _ApplyRotation(dt);
        }
        
        /// <summary>
        /// N unit per second
        /// </summary>
        private readonly float _Speed = 10;
        
        /// <summary>
        /// N degree per second
        /// </summary>
        private readonly float _AngelSpeed = 720;
        #endregion

        public override void AfterModelLoaded()
        {
        }

        public override void Update(float dt)
        { 
            if (_InputSource == ControllerInputSource.None)
            {
                return;
            }
            else if (_InputSource == ControllerInputSource.PlayerInput)
            {
                _CollectAnimatorInputFromPlayerInput();
                _FlushAnimatorInput(dt);
            }
            else if (_InputSource == ControllerInputSource.AI)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void LateUpdate(float dt)
        {
            if (_InputSource == ControllerInputSource.None)
            {
                return;
            }
            else if (_InputSource == ControllerInputSource.PlayerInput)
            {
                _ApplyMotion(dt);
            }
            else if (_InputSource == ControllerInputSource.AI)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        
    }
}