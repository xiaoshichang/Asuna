using System.Collections.Generic;
using System.Runtime.Serialization;
using Asuna.Foundation;

namespace Asuna.Application
{
    
    public enum ControlMsgType
    {
        HandShakeReq = 1,
        HandShakeRsp,
        ConnectGamesNotify,
        GamesConnectedNotify,
        StartupStubsNotify,
        StubReadyNotify,
    }

    [DataContract]
    public class ControlMsg : MsgBase
    {
        public ControlMsg(ControlMsgType cmt)
        {
            MsgType = (int)cmt;
        }
        
    }

    [DataContract]
    public class ControlMsgHandShakeReq : ControlMsg
    {
        public ControlMsgHandShakeReq(string serverName) : base(ControlMsgType.HandShakeReq)
        {
            ServerName = serverName;
        }

        [DataMember]
        public string ServerName;
    }

    [DataContract]
    public class ControlMsgHandShakeRsp : ControlMsg
    {
        public ControlMsgHandShakeRsp(string serverName) : base(ControlMsgType.HandShakeRsp)
        {
            ServerName = serverName;
        }
        
        [DataMember]
        public string ServerName;
    }

    [DataContract]
    public class ControlMsgConnectGamesNotify : ControlMsg
    {
        public ControlMsgConnectGamesNotify() : base(ControlMsgType.ConnectGamesNotify)
        {
        }
    }

    [DataContract]
    public class ControlMsgGamesConnectedNotify : ControlMsg
    {
        public ControlMsgGamesConnectedNotify() : base(ControlMsgType.GamesConnectedNotify)
        {
        }
    }

    [DataContract]
    public class ControlMsgStartupStubsNotify : ControlMsg
    {
        public ControlMsgStartupStubsNotify() : base(ControlMsgType.StartupStubsNotify)
        {
        }
        
        public ControlMsgStartupStubsNotify(ServerStubDistributeTable table) : base(ControlMsgType.StartupStubsNotify)
        {
            foreach (var (key, value) in table.Items)
            {
                if (key.AssemblyQualifiedName == null)
                {
                    Logger.LogError($"AssemblyQualifiedName of {key.Name} is null");
                    continue;
                }
                Items.Add(new KeyValuePair<string, string>(key.AssemblyQualifiedName, value.Name));
            }
        }

        /// <summary>
        /// stub AssemblyQualifiedName - serverName pair
        /// </summary>
        [DataMember]
        public List<KeyValuePair<string, string>> Items = new();
    }

    [DataContract]
    public class ControlMsgStubReadyNotify : ControlMsg
    {
        public ControlMsgStubReadyNotify(string stubName) : base(ControlMsgType.StubReadyNotify)
        {
            StubName = stubName;
        }
        [DataMember] 
        public string StubName;
    }
    
    
}



