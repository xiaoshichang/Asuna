namespace AsunaServer.Foundation
{
    public static class Logger
    {
        public static void Debug(string message)
        {
            Core.Interface.Logger_Debug(message);
        }

        public static void Info(string message)
        {
            Core.Interface.Logger_Info(message);
        }

        public static void Warning(string message)
        {
            Core.Interface.Logger_Warning(message);
        }

        public static void Error(string message)
        {
            Core.Interface.Logger_Error(message);
        }
    }
}

