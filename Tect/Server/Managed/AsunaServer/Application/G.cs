using AsunaServer.Debug;
using AsunaServer.Network;

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
        
        # region Servers
        public static readonly Dictionary<string, TcpSession> ServerToSession = new();
        public static readonly Dictionary<TcpSession, string> SessionToServer = new();
        public static TcpSession? GM;
        public static readonly Dictionary<string, TcpSession> Games = new ();
        public static readonly Dictionary<string, TcpSession> Gates = new ();

        public static bool RegisterSession(string name, TcpSession session)
        {
            Logger.Assert(!ServerToSession.ContainsKey(name));    
            Logger.Assert(!SessionToServer.ContainsKey(session));
            ServerToSession[name] = session;
            SessionToServer[session] = name;

            var config = GroupConfig.GetServerConfigByName(name);
            if (config is GMServerConfig)
            {
                Logger.Assert(GM is null);
                GM = session;
            }
            else if (config is GameServerConfig)
            {
                Logger.Assert(!Games.ContainsKey(name));
                Games.Add(name, session);
            }
            else if (config is GateServerConfig)
            {
                Logger.Assert(!Gates.ContainsKey(name));
                Gates.Add(name, session);
            }
            else
            {
                throw new NotImplementedException();
            }
            return true;
        }

        public static bool UnRegisterSession(TcpSession session)
        {
            Logger.Assert(SessionToServer.ContainsKey(session));
            var name = SessionToServer[session];
            Logger.Assert(ServerToSession.ContainsKey(name));    
            
            var config = GroupConfig.GetServerConfigByName(name);
            if (config is GMServerConfig)
            {
                Logger.Assert(GM is not null);
                GM = null;
            }
            else if (config is GameServerConfig)
            {
                Logger.Assert(Games.ContainsKey(name));
                Games.Remove(name);
            }
            else if (config is GateServerConfig)
            {
                Logger.Assert(Gates.ContainsKey(name));
                Gates.Remove(name);
            }
            else
            {
                throw new NotImplementedException();
            }
            ServerToSession.Remove(name);
            SessionToServer.Remove(session);
            return true;
        }
        # endregion


    }
}

