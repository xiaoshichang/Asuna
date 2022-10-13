using System.Collections.Generic;
using System.Runtime.Serialization;
using Asuna.Foundation.Network;

namespace Asuna.Foundation
{
    
    public enum PayloadMsgType : int
    {
        HandShakeReq = 1,
        HandShakeRsp,
        ConnectGamesNotify,
        GamesConnectedNotify,
        StartupStubsNotify,
        StubReadyNotify,
    }
    
        
    [DataContract]
    public class PayloadMsg
    {
    }

    [DataContract]
    public class PayloadMsgHandShakeReq : PayloadMsg
    {
        public PayloadMsgHandShakeReq(string serverName)
        {
            ServerName = serverName;
        }

        [DataMember]
        public string ServerName;
    }

    [DataContract]
    public class PayloadMsgHandShakeRsp : PayloadMsg
    {
        public PayloadMsgHandShakeRsp(string serverName)
        {
            ServerName = serverName;
        }
        
        [DataMember]
        public string ServerName;
    }

    [DataContract]
    public class PayloadMsgConnectGamesNotify : PayloadMsg
    {
        public PayloadMsgConnectGamesNotify()
        {
        }
    }

    [DataContract]
    public class PayloadMsgGamesConnectedNotify : PayloadMsg
    {
        public PayloadMsgGamesConnectedNotify()
        {
        }
    }

    [DataContract]
    public class PayloadMsgStartupStubsNotify : PayloadMsg
    {
        public PayloadMsgStartupStubsNotify()
        {
        }
        
        public PayloadMsgStartupStubsNotify(ServerStubDistributeTable table)
        {
            foreach (var (key, value) in table.Items)
            {
                if (key.AssemblyQualifiedName == null)
                {
                    ALogger.LogError($"AssemblyQualifiedName of {key.Name} is null");
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
    public class PayloadMsgStubReadyNotify : PayloadMsg
    {
        public PayloadMsgStubReadyNotify(string stubName)
        {
            StubName = stubName;
        }
        [DataMember] 
        public string StubName;
    }
    
    
}



