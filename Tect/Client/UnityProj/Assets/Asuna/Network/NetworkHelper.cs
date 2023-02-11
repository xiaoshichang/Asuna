
using System;
using System.Net;

namespace Asuna.Network
{
    public static class NetworkHelper
    {
        public static IPEndPoint ParseIPEndPoint(string endPoint)
        {
            if (!endPoint.Contains(":"))
            {
                throw new Exception("invalid format");
            }

            var parts = endPoint.Split(":");
            if (parts.Length != 2)
            {
                throw new Exception("invalid format");
            }

            if (!IPAddress.TryParse(parts[0], out IPAddress address))
            {
                throw new Exception("invalid format");
            }

            if (!Int32.TryParse(parts[1], out int port))
            {
                throw new Exception("invalid format");
            }

            return new IPEndPoint(address, port);
        }
    }
}