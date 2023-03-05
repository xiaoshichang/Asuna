using System.Reflection;
using System.Text.Json.Serialization;
using AsunaServer.Foundation.Debug;
using AsunaServer.Entity;
using AsunaServer.Message;
using AsunaServer.Network;
using AsunaServer.Utils;
using Google.Protobuf;
using Google.Protobuf.Collections;

#pragma warning disable CS8618

namespace AsunaServer.Application
{
    public static class G
    {

        #region Config
        /// <summary>
        /// 服务器组配置
        /// </summary>
        public static ServerGroupConfig GroupConfig;
        
        /// <summary>
        /// 本服务器配置
        /// </summary>
        public static ServerConfigBase ServerConfig;
        #endregion

        /// <summary>
        /// 全局网络消息序列化器
        /// </summary>
        public static SerializerBase MessageSerializer = new ProtobufSerializer();
    }
}

