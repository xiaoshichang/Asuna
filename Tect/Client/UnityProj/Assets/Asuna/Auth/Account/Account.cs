using System;
using System.Collections.Generic;
using Asuna.Network;
using AsunaShared.Message;

namespace Asuna.Auth
{
    public class Account
    {
        public Guid Guid;
        public List<Guid> AvatarList = new List<Guid>();

        public Account(AccountData data)
        {
            Guid = data.Guid.ToGuid();
            foreach (var item in data.AvatarList)
            {
                AvatarList.Add(item.ToGuid());
            }
        }
    }
}