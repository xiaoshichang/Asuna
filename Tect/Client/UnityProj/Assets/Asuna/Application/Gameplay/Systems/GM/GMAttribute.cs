using System;

namespace Asuna.Application.GM
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class GMAttribute : Attribute
    {
        public GMAttribute(string command, string desc)
        {
            Command = command;
            Desc = desc;
        }

        /// <summary>
        /// GM命令
        /// </summary>
        public string Command;

        /// <summary>
        /// GM命令描述
        /// </summary>
        public readonly string Desc;
    }
}