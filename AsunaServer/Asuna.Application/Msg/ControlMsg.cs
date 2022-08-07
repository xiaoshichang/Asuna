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
    
    
}



