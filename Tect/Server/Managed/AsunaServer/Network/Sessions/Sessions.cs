
using AsunaServer.Application;
using AsunaServer.Foundation.Debug;
using AsunaServer.Utils;

namespace AsunaServer.Network
{
    public static class Sessions
    {
        public static readonly Dictionary<string, TcpSession> ServerToSession = new();
        public static readonly Dictionary<TcpSession, string> SessionToServer = new();
        public static TcpSession? GM;
        public static readonly Dictionary<string, TcpSession> Games = new();
        public static readonly Dictionary<string, TcpSession> Gates = new();

        public static bool RegisterSession(string name, TcpSession session)
        {
            ADebug.Assert(!ServerToSession.ContainsKey(name));
            ADebug.Assert(!SessionToServer.ContainsKey(session));
            ServerToSession[name] = session;
            SessionToServer[session] = name;

            var config = G.GroupConfig.GetServerConfigByName(name);
            if (config is GMServerConfig)
            {
                ADebug.Assert(GM is null);
                GM = session;
            }
            else if (config is GameServerConfig)
            {
                ADebug.Assert(!Games.ContainsKey(name));
                Games.Add(name, session);
            }
            else if (config is GateServerConfig)
            {
                ADebug.Assert(!Gates.ContainsKey(name));
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
            ADebug.Assert(SessionToServer.ContainsKey(session));
            var name = SessionToServer[session];
            ADebug.Assert(ServerToSession.ContainsKey(name));

            var config = G.GroupConfig.GetServerConfigByName(name);
            if (config is GMServerConfig)
            {
                ADebug.Assert(GM is not null);
                GM = null;
            }
            else if (config is GameServerConfig)
            {
                ADebug.Assert(Games.ContainsKey(name));
                Games.Remove(name);
            }
            else if (config is GateServerConfig)
            {
                ADebug.Assert(Gates.ContainsKey(name));
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

        public static TcpSession? GetSessionByName(string name)
        {
            if (ServerToSession.TryGetValue(name, out var session))
            {
                return session;
            }

            return null;
        }
    }
}
