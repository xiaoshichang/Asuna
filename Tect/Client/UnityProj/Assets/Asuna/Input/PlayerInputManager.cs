using System.Collections.Generic;
using Asuna.Interface;
using Asuna.Utils;
using InControl;

namespace Asuna.Input
{
    /// <summary>
    /// 玩家枚举
    /// </summary>
    public enum PlayerType
    {
        Player1 = 0,
        Player2,
        Player3,
        Player4,
        Count
    }
    
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
            _RemoveAllPlayerInputMapping();
            _UnregisterDeviceEvents();
        }

        public void Update(float dt)
        {
        }
        
        #region Device events

        private void _SearchDevices()
        {
            foreach (var device in InputManager.Devices)
            {
                ADebug.Info($"Device found: {device.Name} | {device.DeviceClass} | {device.GUID}");
            }
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

        public InputDevice GetAvailableDevice()
        {
            foreach (var device in InputManager.Devices)
            {
                if (IsDeviceUsed(device))
                {
                    continue;
                }

                return device;
            }

            return null;
        }

        private bool IsDeviceUsed(InputDevice device)
        {
            foreach (var mapping in _AllPlayersInputMapping.Values)
            {
                if (mapping.GetRelativeDevice() == device)
                {
                    return true;
                }
            }

            return false;
        }
        
        #endregion

        #region Player Input

        public void SetupPlayerInputMapping(PlayerType playerType, PlayerInputMapping mapping)
        {
            ADebug.Assert(playerType != PlayerType.Count);
            ADebug.Assert(!_AllPlayersInputMapping.ContainsKey(playerType));
            _AllPlayersInputMapping[playerType] = mapping;
        }

        private void _RemoveAllPlayerInputMapping()
        {
            foreach (var playerType in _AllPlayersInputMapping.Keys)
            {
                _AllPlayersInputMapping[playerType].Release();
            }
            _AllPlayersInputMapping.Clear();
        }
        
        public void RemovePlayerInputMapping(PlayerType playerType)
        {
            ADebug.Assert(playerType != PlayerType.Count);
            ADebug.Assert(_AllPlayersInputMapping.ContainsKey(playerType));
            _AllPlayersInputMapping.Remove(playerType, out var mapping);
            mapping.Release();
        }

        public PlayerInputMapping GetMapping(PlayerType playerType)
        {
            if (_AllPlayersInputMapping.TryGetValue(playerType, out var mapping))
            {
                return mapping;
            }

            return null;
        }

        private readonly Dictionary<PlayerType, PlayerInputMapping> _AllPlayersInputMapping = new();

        #endregion

    }
}