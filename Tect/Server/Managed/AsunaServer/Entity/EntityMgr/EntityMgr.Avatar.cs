using System.Reflection;
using AsunaServer.Foundation.Debug;

namespace AsunaServer.Entity;

public static partial class EntityMgr
{
    public static void RegisterAvatar(List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.Name == "Avatar")
                {
                    _AvatarType = type;
                }
            }
        }

        if (_AvatarType == null)
        {
            ADebug.Error("Avatar not found");
        }
    }

    private static Type? _AvatarType;
}