using AsunaServer.Core;

namespace AsunaServer.Foundation;


public static class TimerMgr
{
    public static uint AddTimer(uint delayMs, TimeoutCallback callback, object? param)
    {
        var timer = new Timer(delayMs, callback, param);
        _Timers[timer.TimerID] = timer;
        return timer.TimerID;
    }

    public static uint AddRepeatTimer(uint delayMs, uint intervalMs, TimeoutCallback callback, object param)
    {
        var timer = new Timer(delayMs, intervalMs, callback, param);
        _Timers[timer.TimerID] = timer;
        return timer.TimerID;
    }
    

    public static bool CancelTimer(uint tid)
    {
        if (!_Timers.TryGetValue(tid, out var timer))
        {
            Logger.Warning($"timer({tid}) not exist");
            return false;
        }

        timer?.Cancel();
        _Timers.Remove(tid);
        return true;
    }

    public static void OnTimeout(Timer timer)
    {
        if (!timer.IsRepeat)
        {
            _Timers.Remove(timer.TimerID);
        }
    }

    public static int GetTimersCount()
    {
        var internalTimersCount = Interface.Timer_GetTimersCount();
        if (internalTimersCount != _Timers.Count)
        {
            Logger.Error("timers count not match");
        }
        
        return _Timers.Count;
    }

    private static readonly Dictionary<uint, Timer> _Timers = new();

}