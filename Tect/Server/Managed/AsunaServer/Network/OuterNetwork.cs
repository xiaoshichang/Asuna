﻿using AsunaServer.Core;
using AsunaServer.Debug;
using AsunaServer.Message.Serializer;

namespace AsunaServer.Network
{
    public static class OuterNetwork
    {
        public static void Init(string ip, int port, OnAcceptCallback? onAccept, OnConnectCallback? onConnect, OnReceiveCallback? onReceive)
        {
            _OnAcceptCallback = onAccept;
            _OnConnectCallback = onConnect;
            _OnReceiveCallback = onReceive;
            Interface.OuterNetwork_Init(ip, port, _OnAccept, _OnDisconnect);
        }
        
        private static void _OnReceiveMessage(IntPtr connection, object message)
        {
            if (_Sessions.TryGetValue(connection, out var session))
            {
                _OnReceiveCallback?.Invoke(session, message);
                return;
            }
            Logger.Warning("OnReceiveMessage connection does not exist!");
        }
        
        private static void _OnAccept(IntPtr connection)
        {
            Logger.Debug($"Outer Network OnAccept {connection}");
            if (_Sessions.ContainsKey(connection))
            {
                Logger.Error($"session({connection}) already exist.");
                return;
            }
            
            var session = new TcpSession(connection, false, _OnReceiveMessage);
            _Sessions.Add(connection, session);
            _OnAcceptCallback?.Invoke(session);
        }

        private static void _OnDisconnect(IntPtr connection)
        {
            Logger.Debug($"Outer Network OnDisconnect {connection}");
            if (_Sessions.TryGetValue(connection, out var session))
            {
                session.OnDisconnect();
                _Sessions.Remove(connection);
            }
            else
            {
                Logger.Warning("session not exist.");
            }
        }

        /// <summary>
        /// onAccept上层业务回调
        /// </summary>
        private static OnAcceptCallback? _OnAcceptCallback;

        /// <summary>
        /// onReceive上层业务回调
        /// </summary>
        private static OnReceiveCallback? _OnReceiveCallback;

        /// <summary>
        /// onConnect上层业务回调
        /// </summary>
        private static OnConnectCallback? _OnConnectCallback;
        
        /// <summary>
        /// 维护所有有效链接
        /// </summary>
        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();

        /// <summary>
        /// 使用的序列化器
        /// </summary>
        public static SerializerBase Serializer = new ProtobufSerializer();
    }
}

