
using System;
using System.Runtime.Serialization;

#pragma warning disable CS8618

namespace Asuna.Foundation.Network
{
    public enum PackageType
    {
        Json = 1,
    }

    /// <summary>
    /// 
    ///     +-------------------------------------------------------------------------------------------+
    ///     +   Package Type(4 bytes)     |    Payload Size(4 bytes)   |   Payload(data_size bytes)     +
    ///     +-------------------------------------------------------------------------------------------+
    /// 
    /// </summary>
    public abstract class PackageBase
    {
        public PackageType PackageType;
        public int PayloadSize;
        public byte[] Payload;
        public int PayloadOffset;

        public abstract byte[] DumpPayload();
        public abstract int GetPayloadMsgType();
        public abstract T ParsePayload<T>();
        
        public static void ParseHeader(byte[] buffer, out PackageType packageType, out int payloadSize)
        {
            if (buffer.Length < PackageHeaderSize)
            {
                throw new Exception("Buffer size too small");
            }

            packageType = (PackageType)BitConverter.ToInt32(buffer, 0);
            payloadSize = BitConverter.ToInt32(buffer, 4);
        }


        public byte[] DumpPackage()
        {
            var payload = DumpPayload();
            
            byte[] HeaderBuffer = new byte[PackageHeaderSize];
            var packageType = BitConverter.GetBytes((int)PackageType);
            var payloadSize = BitConverter.GetBytes(payload.Length);
            Array.Copy(packageType, 0, HeaderBuffer, 0, 4);
            Array.Copy(payloadSize, 0, HeaderBuffer, 4, 4);
            
            byte[] packageBuffer = new byte[PackageHeaderSize + payload.Length];
            Array.Copy(HeaderBuffer, 0, packageBuffer, 0, HeaderBuffer.Length);
            Array.Copy(payload, 0, packageBuffer, HeaderBuffer.Length, payload.Length);
            return packageBuffer;
        }


        public const int PackageHeaderSize = 8;
    }
    
    
}