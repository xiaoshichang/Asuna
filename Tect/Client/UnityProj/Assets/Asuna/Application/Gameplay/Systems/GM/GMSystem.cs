﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;


namespace Asuna.Application.GM
{
    
    public class GMSystem : ISystem
    {
        /// <summary>
        /// 初始化系统
        /// </summary>
        /// <param name="param">包含GM指令的Assembly列表</param>
        public void Init(object param)
        {
            var assemblyList = param as List<string>;
            if (assemblyList is null)
            {
                return;
            }
            
            foreach (var assemblyName in assemblyList)
            {
                _CollectGMCommandsByReflection(assemblyName);
            }
        }
        
        public void Release()
        {
            _AllGMCommands.Clear();
        }
        
        
        private void _RegisterGMCommand(GMAttribute attr, MethodInfo method)
        {
            if (_AllGMCommands.ContainsKey(method.Name))
            {
                XDebug.Error($"duplicated gm name {method.Name}");
                return;
            }

            var cmd = new GMCommand()
            {
                Attribute = attr,
                Method = method
            };
            _AllGMCommands.Add(method.Name, cmd);
        }
        
        /// <summary>
        /// 从Assembly中搜集GM指令
        /// </summary>
        private void _CollectGMCommandsByReflection(string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            foreach (var t in assembly.GetTypes())
            {
                foreach (var method in t.GetMethods())
                {
                    var attrs = method.GetCustomAttributes();
                    foreach (var attr in attrs)
                    {
                        if (attr is GMAttribute gma)
                        {
                            _RegisterGMCommand(gma, method);
                        }
                    }
                }
            }
        }

        private object _ConvertParameter(string item, Type t)
        {
            try
            {
                if (t == typeof(int))
                {
                    return int.Parse(item);
                }

                if (t == typeof(float))
                {
                    return float.Parse(item);
                }

                if (t == typeof(string))
                {
                    return item;
                }

                if (t == typeof(uint))
                {
                    return uint.Parse(item);
                }

                if (t == typeof(double))
                {
                    return double.Parse(item);
                }
            }
            catch (Exception e)
            {
                XDebug.Error($"{e.Message}! Content:{item}), Type:{t.Name}");
            }
            return null;
        }

        private void _PrintCandidates(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(">>>>>>>>>> relative commands >>>>>>>>>>\n");
            foreach (var kv in _AllGMCommands)
            {
                if (kv.Key.StartsWith(prefix))
                {
                    var cmd = kv.Value;
                    sb.Append($"{kv.Key} - {cmd.Attribute.Desc} \n");
                }
            }
            sb.Append("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n");
            XDebug.Info(sb.ToString());
        }

        /// <summary>
        /// 执行GM命令
        /// </summary>
        public bool Execute(string input)
        {
            var items = input.Split();
            if (items.Length <= 0)
            {
                XDebug.Error("gm input invalid!");
                return false;
            }
            var cmdName = items[0];
            if (!_AllGMCommands.TryGetValue(cmdName, out var cmd))
            {
                XDebug.Error($"command [{cmdName}] not found!");
                _PrintCandidates(cmdName);
                return false;
            }

            var infos = cmd.Method.GetParameters();
            var count = infos.Length;
            if (count != items.Length - 1)
            {
                XDebug.Error("gm parameter count not match!");
                return false;
            }

            var parameters = new object[count];
            for (var i = 0; i < count; i++)
            {
                var info = infos[i];
                var content = items[i + 1];
                var parameter = _ConvertParameter(content, info.ParameterType);
                if (parameter == null)
                {
                    return false;
                }
                parameters[i] = parameter;
            }
            
            try
            {
                cmd.Method.Invoke(null, parameters);
                return true;
            }
            catch (Exception e)
            {
                XDebug.Error(e.Message);
                return false;
            }
        }

        private readonly Dictionary<string, GMCommand> _AllGMCommands = new();
        
    }
}