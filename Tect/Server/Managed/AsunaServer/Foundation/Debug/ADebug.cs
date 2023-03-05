
namespace AsunaServer.Foundation.Debug
{
    public static class ADebug
    {
        public static void Init(string target, string fileName)
        {
            Core.Interface.Logger_Init(target, fileName);
        }
        
        public static void Info(string format, params object[] args)
        {
            Core.Interface.Logger_Info(string.Format(format, args));
        }

        public static void Warning(string format, params object[] args)
        {
            Core.Interface.Logger_Warning(string.Format(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            Core.Interface.Logger_Error(string.Format(format, args));
        }

        public static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception();
            }
        }

        public static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception(message);
            }
        }
    }
}

