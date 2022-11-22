using System;
using UnityEngine;
using Debugger = UnityEngine.Debug;


namespace AsunaClient.Foundation
{
    public enum LogLevel : byte
    {
        Debug,
        Info,
        Warning,
        Error
    }
    
    
    public static class Logger
    {
        private static string _RecordFormat = "[{0}][{1}] - {2}";
       
        public static void Init()
        {
        }

        public static void Info(string format, params object[] args)
        {
            _Log(LogLevel.Info, string.Format(format, args));
        }

        public static void Warning(string format, params object[] args)
        {
            _Log(LogLevel.Warning, string.Format(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            _Log(LogLevel.Error, string.Format(format, args));

        }
        
        private static void _Log(LogLevel level, string message)
        {
            var ts =  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            var record = string.Format(_RecordFormat, ts, level.ToString(), message);
            _SendToConsole(level, record);
        }
        
        private static void _SendToConsole(LogLevel level, string record)
        {
            if (level == LogLevel.Debug)
            {
                Debugger.Log(record);
            }
            else if (level == LogLevel.Info)
            {
                Debugger.Log(record);
            }
            else if (level == LogLevel.Warning)
            {
                Debugger.LogWarning(record);
            }
            else if (level == LogLevel.Error)
            {
                Debugger.LogError(record);
            }
            else
            {
                throw new NotImplementedException("unknown log level");
            }
        }
        
    }
}