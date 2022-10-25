using System.Runtime.InteropServices;

namespace XServer.Core
{
    public static partial class Interface
    {
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void Server_Init();
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void Server_Run();
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void Server_Finalize();
        

        
        

    }    
}
