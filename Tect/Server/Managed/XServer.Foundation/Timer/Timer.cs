using XServer.Core;

namespace XServer.Foundation
{
    public delegate void TimeoutCallback(object? param);
    
    
    public class Timer
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
}

