using System.Reflection;
using AsunaServer.Foundation.Debug;
using AsunaServer.Utils;

namespace AsunaServer.Entity;

public static partial class EntityMgr
{

    private static void _RegisterAvatarType(Type type)
    {
        if (_AvatarType != null)
        {
            ADebug.Warning("more than one Avatar Type");
            return;
        }

        _AvatarType = type;
    }
    
    /// <summary>
    /// 注册所有ServerEntity类型
    /// </summary>
    public static void RegisterServerEntityTypes(List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsSubclassOf(typeof(ServerEntity)))
                {
                    continue;
                }

                if (type.IsSubclassOf(typeof(ServerStubEntity)))
                {
                    continue;
                }
                
                if (type.IsSubclassOf(typeof(AvatarBase)))
                {
                    _RegisterAvatarType(type);
                }

                var index = HashUtils.ClassToUint(type);
                ADebug.Assert(!_IndexToServerEntityType.ContainsKey(index));
                _IndexToServerEntityType[index] = type;
            }
        }
    }

    /// <summary>
    /// 根据索引获取对应类型
    /// </summary>
    public static Type? GetServerEntityTypeByIndex(uint index)
    {
        if (_IndexToServerEntityType.TryGetValue(index, out var t))
        {
            return t;
        }
        
        return null;
    }

    public static Type? GetAvatarType()
    {
        return _AvatarType;
    }

    private static readonly Dictionary<uint, Type> _IndexToServerEntityType = new();
    private static Type? _AvatarType = null;

}