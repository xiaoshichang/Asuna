﻿using System.Runtime.InteropServices;
namespace AsunaServer.Core;

public static partial class Interface
{
    
//https://blog.magnusmontin.net/2018/11/05/platform-conditional-compilation-in-net-core/
#if Linux
    private const string _DllPath = @"/home/xiao/CLionProjects/XServer/Core/cmake-build-debug/libXServer.so";
#elif Windows
    private const string _DllPath = @"C:\Users\xiao\Documents\GitHub\Asuna\Tect\Server\Core\out\build\x64-Debug\AsunaServer.dll";
#endif
    
    
    private const CallingConvention _CallingConvention = CallingConvention.Winapi;
    private const CharSet _CharSet = CharSet.Ansi;
}
