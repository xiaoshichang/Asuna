using System.Runtime.InteropServices;
namespace AsunaServer.Core;

public static partial class Interface
{
    
//https://blog.magnusmontin.net/2018/11/05/platform-conditional-compilation-in-net-core/
#if Linux
    private const string _DllPath = @"../libXServer.so";
#elif Windows
    private const string _DllPath = @"..\AsunaServer.dll";
#endif
    
    
    private const CallingConvention _CallingConvention = CallingConvention.Winapi;
    private const CharSet _CharSet = CharSet.Ansi;
}
