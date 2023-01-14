﻿namespace AsunaServer.Debug
{
    public static class Logger
    {
        public static void Init(string target, string fileName)
        {
            Core.Interface.Logger_Init(target, fileName);
        }
        
        public static void Debug(string format, params object[] args)
        {
            Core.Interface.Logger_Debug(string.Format(format, args));
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
    }
}
