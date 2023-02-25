using System.Collections.Generic;
using Asuna.Interface;
using Asuna.Utils;
using InControl;

namespace Asuna.Input
{
    
    /// <summary>
    /// 玩家的输入管理
    /// </summary>
    public class PlayerInputManager : IManager
    {

        public void Init(object param)
        {
            _SearchDevices();
            _RegisterDeviceEvents();
        }

        
        public void Release()
        {
            _UnregisterDeviceEvents();
        }

        public void Update(float dt)
        {
        }
        
        private void _SearchDevices()
        {
            foreach (var device in InputManager.Devices)
            {
                ADebug.Info($"Device found: {device.Name} | {device.DeviceClass} | {device.GUID}");
            }
        }

        public InputDevice GetAvailableDevice()
        {
            foreach (var device in InputManager.Devices)
            {
                return device;
            }

            return null;
        }
        
        
        private void _RegisterDeviceEvents()
        {
            InputManager.OnDeviceAttached += _OnDeviceAttach;
            InputManager.OnDeviceDetached += _OnDeviceDetach;
            InputManager.OnActiveDeviceChanged += _OnActiveDeviceChange;
        }

        private void _UnregisterDeviceEvents()
        {
            InputManager.OnDeviceAttached -= _OnDeviceAttach;
            InputManager.OnDeviceDetached -= _OnDeviceDetach;
            InputManager.OnActiveDeviceChanged -= _OnActiveDeviceChange;
        }

        private void _OnDeviceAttach(InputDevice device)
        {
            ADebug.Info($"_OnDeviceAttach {device.Name} | {device.DeviceClass} | {device.GUID}");
        }

        private void _OnDeviceDetach(InputDevice device)
        {
            ADebug.Info($"_OnDeviceDetach {device.Name} | {device.DeviceClass} | {device.GUID}");
        }

        private void _OnActiveDeviceChange(InputDevice device)
        {
            ADebug.Info($"_OnActiveDeviceChange {device.Name} | {device.DeviceClass} | {device.GUID}");
        }

        /// <summary>
        /// 设置输入映射关系
        /// </summary>
        public void SetupPlayerInputMapping(PlayerInputMapping mapping)
        {
            _PlayerInputMapping = mapping;
        }

        /// <summary>
        /// 移除所有输入映射关系
        /// </summary>
        public void ClearPlayerInputMapping()
        {
            _PlayerInputMapping = null;
        }

        public PlayerInputMapping GetPlayerInputMapping()
        {
            return _PlayerInputMapping;
        }

        private PlayerInputMapping _PlayerInputMapping;

    }
}