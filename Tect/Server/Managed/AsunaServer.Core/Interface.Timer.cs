using System.Runtime.InteropServices;

namespace AsunaServer.Core;


public delegate void TimerCallback();


public static partial class Interface
{
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern uint Timer_AddTimer(uint delay, TimerCallback callback);
    
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern uint Timer_AddRepeatTimer(uint delay, uint interval, TimerCallback callback);
    
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern bool Timer_CancelTimer(uint tid);
    
    [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
    public static extern uint Timer_GetTimersCount();
}