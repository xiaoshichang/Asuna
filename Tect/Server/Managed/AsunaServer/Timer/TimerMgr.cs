using AsunaServer.Core;
using AsunaServer.Debug;


namespace AsunaServer.Timer
{
    public delegate void TimeoutCallback(object? param);

    
    public static class TimerMgr
    {
        internal class Timer
        {
            public Timer(uint delayMS, TimeoutCallback callback, object? param)
            {
                IsRepeat = false;
                _TimeoutCallback = callback;
                _Param = param;
                TimerID = Interface.Timer_AddTimer(delayMS, OnTimeout);
            }

            public Timer(uint delayMS, uint intervalMS, TimeoutCallback callback, object? param)
            {
                IsRepeat = true;
                _TimeoutCallback = callback;
                _Param = param;
                TimerID = Interface.Timer_AddRepeatTimer(delayMS, intervalMS, OnTimeout);
            }

            private void OnTimeout()
            {
                _TimeoutCallback.Invoke(_Param);
                TimerMgr.OnTimeout(this);
            }

            public bool Cancel()
            {
                return Interface.Timer_CancelTimer(TimerID);
            }


            public readonly uint TimerID;
            public readonly bool IsRepeat;
            private readonly TimeoutCallback _TimeoutCallback;
            private readonly object? _Param;

        }
        
        
        public static uint AddTimer(uint delayMs, TimeoutCallback callback, object? param)
        {
            var timer = new Timer(delayMs, callback, param);
            _Timers[timer.TimerID] = timer;
            return timer.TimerID;
        }

        public static uint AddTimer(uint delayMs, TimeoutCallback callback)
        {
            return TimerMgr.AddTimer(delayMs, callback, null);
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
    
        internal static void OnTimeout(Timer timer)
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
}


