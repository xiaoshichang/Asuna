using System;
using Codice.CM.Common;
using UnityEngine;

namespace AsunaClient.Foundation
{
    public enum LogLevel : byte
    {
        Debug,
        Info,
        Warning,
        Error
    }
    
    public static partial class XDebug
    {
        private static readonly string _RecordFormat = "[{0}][{1}] - {2}";
       
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

        public static void Asset(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception(message);
            }
        }

        public static void Asset(bool condition)
        {
            if (!condition)
            {
                throw new Exception();
            }
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
                Debug.Log(record);
            }
            else if (level == LogLevel.Info)
            {
                Debug.Log(record);
            }
            else if (level == LogLevel.Warning)
            {
                Debug.LogWarning(record);
            }
            else if (level == LogLevel.Error)
            {
                Debug.LogError(record);
            }
            else
            {
                throw new NotImplementedException("unknown log level");
            }
        }
    }
}