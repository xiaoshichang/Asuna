#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf;

namespace Asuna.Message
{
    public class ProtobufSerializer : SerializerBase
    {
        public override byte[] Serialize(object obj)
        {
            var message = obj as IMessage;
            return message.ToByteArray();
        }

        public override object Deserialize(byte[] data, int length, uint typeIndex)
        {
            if (_Index2Parsers.TryGetValue(typeIndex, out var parser))
            {
                return parser.ParseFrom(data, 0, length);
            }
            throw new Exception($"Deserialize error! type:{typeIndex}");
        }

        private MessageParser? _GetParserByType(Type type)
        {
            var propertyInfo = type.GetProperty("Parser");
            if (propertyInfo == null)
            {
                return null;
            }

            var getter = propertyInfo.GetGetMethod();
            if (getter == null)
            {
                return null;
            }

            var parser = getter.Invoke(null, null);
            return parser as MessageParser;
        }
    
        public override void Collect(List<Assembly> assemblies)
        {
            var baseType = typeof(IMessage);
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.GetInterface(baseType.Name) != null)
                    {
                        _Register(type);
                    }
                }
            }
        }
    
        protected override void _Register(Type type)
        {
            uint index = _ConvertTypeToHash(type);
            if (_Index2Type.ContainsKey(index))
            {
                throw new Exception($"duplicated index register. {type.Name}");
            }

            _Type2Index[type] = index;
            _Index2Type[index] = type;

            var parser = _GetParserByType(type);
            if (parser == null)
            {
                throw new Exception($"parser not found. {type.Name}");
            }
            _Index2Parsers[index] = parser;
        }

        private readonly Dictionary<uint, MessageParser> _Index2Parsers = new();


    }
}