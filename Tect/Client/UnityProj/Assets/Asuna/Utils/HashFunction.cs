using System.Reflection;

namespace Asuna.Utils
{
    public static class HashFunction
    {
    
        public static uint StringToUint(string str)
        {
            uint hashedValue = 0;
            uint multiplier = 1;
            int i = 0;
            while (i < str.Length)
            {
                hashedValue += str[i] * multiplier;
                multiplier *= 37;
                i++;
            }
            return hashedValue;
        }

        public static uint RpcToUint(string typeName, string methodName)
        {
            var full = $"{typeName}.{methodName}";
            return StringToUint(full);
        }
        
    }
}