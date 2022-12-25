using System;
using Codice.CM.Common;
using UnityEngine;

namespace AF.Utils
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

        /// <summary>
        /// Warning  代表发生了错误，但是不影响主流程和底层模块，只影响部分业务模块
        /// </summary>
        public static void Warning(string format, params object[] args)
        {
            _Log(LogLevel.Warning, string.Format(format, args));
        }

        /// <summary>
        /// Error 代表发生严重错误，正常的流程不允许存在
        /// </summary>
        public static void Error(string format, params object[] args)
        {
            _Log(LogLevel.Error, string.Format(format, args));
        }

        /// <summary>
        /// Asset 代表程序员的预期，不符合预期直接抛出异常，避免错误继续
        /// </summary>
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
                UnityEngine.Debug.Log(record);
            }
            else if (level == LogLevel.Info)
            {
                UnityEngine.Debug.Log(record);
            }
            else if (level == LogLevel.Warning)
            {
                UnityEngine.Debug.LogWarning(record);
            }
            else if (level == LogLevel.Error)
            {
                UnityEngine.Debug.LogError(record);
            }
            else
            {
                throw new Exception("unknown log level");
            }
        }
    }
}