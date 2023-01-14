using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Asuna.Interface;
using Asuna.Utils;
using UnityEngine;


namespace Asuna.Gameplay
{
    
    public class GMSystem : ISystem
    {
        /// <summary>
        /// 初始化系统
        /// </summary>
        /// <param name="param">包含GM指令的Assembly列表</param>
        public void Init(object param)
        {
            // collect from framework
            CollectGMCommandsByReflection(Assembly.GetExecutingAssembly());
            
            // collect from gameplay
            var assemblyList = param as List<string>;
            if (assemblyList is null)
            {
                return;
            }
            foreach (var assemblyName in assemblyList)
            {
                var assembly = Assembly.Load(assemblyName);
                CollectGMCommandsByReflection(assembly);
            }
        }
        
        public void Release()
        {
            _AllGMCommands.Clear();
        }
        
        
        private void _RegisterGMCommand(GMAttribute attr, MethodInfo method)
        {
            if (_AllGMCommands.ContainsKey(attr.Command))
            {
                ADebug.Error($"duplicated gm name {method.Name}");
                return;
            }

            var cmd = new GMCommand()
            {
                Attribute = attr,
                Method = method
            };
            _AllGMCommands.Add(attr.Command, cmd);
        }
        
        /// <summary>
        /// 从Assembly中搜集GM指令
        /// </summary>
        public void CollectGMCommandsByReflection(Assembly assembly)
        {
            if (assembly == null)
            {
                ADebug.Error("assembly is null");
                return;
            }
            
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
                ADebug.Error($"{e.Message}! Content:{item}), Type:{t.Name}");
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
            ADebug.Info(sb.ToString());
        }

        /// <summary>
        /// 执行GM命令
        /// </summary>
        public bool Execute(string input)
        {
            var items = input.Split();
            if (items.Length <= 0)
            {
                ADebug.Error("gm input invalid!");
                return false;
            }
            var cmdName = items[0];
            if (!_AllGMCommands.TryGetValue(cmdName, out var cmd))
            {
                ADebug.Warning($"command [{cmdName}] not found!");
                _PrintCandidates(cmdName);
                return false;
            }

            var infos = cmd.Method.GetParameters();
            var count = infos.Length;
            if (count != items.Length - 1)
            {
                ADebug.Error("gm parameter count not match!");
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
                ADebug.Error(e.Message);
                return false;
            }
        }

        public int GetCommandsCount()
        {
            return _AllGMCommands.Count;
        }

        private readonly Dictionary<string, GMCommand> _AllGMCommands = new();
        
    }
}