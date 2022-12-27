﻿using System;

namespace Asuna.Application.GM
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class GMAttribute : Attribute
    {
        public GMAttribute(string desc)
        {
            Desc = desc;
        }

        /// <summary>
        /// GM命令描述
        /// </summary>
        public string Desc;
    }
}