using System.Runtime.InteropServices;

namespace AsunaServer.Core;

public static partial class Interface
{
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern void Logger_Debug(string message);
        
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern void Logger_Info(string message);
        
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern void Logger_Warning(string message);
        
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern void Logger_Error(string message);

}