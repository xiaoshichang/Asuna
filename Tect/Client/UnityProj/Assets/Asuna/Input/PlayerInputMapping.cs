﻿using Asuna.Utils;
using InControl;

namespace Asuna.Input
{
    /// <summary>
    /// 代表和管理一个玩家的输入
    /// 包括多个设备，以及每个输入代表的逻辑意义
    /// </summary>
    public class PlayerInputMapping
    {
        public PlayerInputMapping(InputDevice device, PlayerActionSet actionSet)
        {
            _Device = device;
            _ActionSet = actionSet;

            if (_Device != null)
            {
                _ActionSet.Device = _Device;    // 指定这个ActionSet的输入源
            }
        }

        public void Release()
        {
            _ActionSet.Destroy();
        }

        public InputDevice GetRelativeDevice()
        {
            return _Device;
        }

        public PlayerActionSet GetActionSet()
        {
            return _ActionSet;
        }

        private readonly InputDevice _Device;
        private readonly PlayerActionSet _ActionSet;


    }

 
}