using System.Reflection;

#pragma warning disable

namespace AsunaServer.Utils
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

        public static uint MethodToUint(MethodInfo method)
        {
            var type = method.DeclaringType;
            var full = $"{type.Name}.{method.Name}";
            return StringToUint(full);
        }
        
    }
}
