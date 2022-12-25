﻿using System;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

namespace AsunaClient.Foundation
{
    public delegate void TimeoutCallback(object? arg);

    public static class TimerMgr
    {
        private class TimerCompare : IComparer<Timer>
        {
            public int Compare(Timer? x, Timer? y)
            {
                if (x == null || y == null)
                {
                    throw new NullReferenceException();
                }
                return x.NextTimeout.CompareTo(y.NextTimeout);
            }
        }
        private class Timer
        {
            public Timer(bool isRepeat, ulong delayInMilliSeconds, TimeoutCallback callback, object? arg)
            {
                TimerID = NextTimerID();
                IsRepeat = isRepeat;
                _Delay = delayInMilliSeconds;
                NextTimeout = _Delay + TimeUtils.GetTimeStampInMilliseconds();
                _Arg = arg;
                _Callback = callback;
            }

            public bool IsTimeout(ulong timestamp)
            {
                return timestamp >= NextTimeout;
            }

            public void OnTimeout()
            {
                _Callback.Invoke(_Arg);
            }

            private static ulong NextTimerID()
            {
                GlobalTimerID += 1;
                if (GlobalTimerID == ulong.MaxValue)
                {
                    GlobalTimerID = 1;
                }
                return GlobalTimerID;
            }

            public void Repeat()
            {
                NextTimeout += _Delay;
            }

            public readonly ulong TimerID;
            public bool IsRepeat { get; }
            public ulong NextTimeout;
            private ulong _Delay { get; }
            private readonly TimeoutCallback _Callback;
            private readonly object? _Arg;
            private static ulong GlobalTimerID = 0;
        }
        
        public static ulong RegisterTimer(bool repeat, ulong delayInMilliSeconds, TimeoutCallback callback, object? arg)
        {
            var timer = new Timer(repeat, delayInMilliSeconds, callback, arg);
            _Timers.Add(timer);
            _Tid2Timer.Add(timer.TimerID, timer);
            return timer.TimerID;
        }

        public static ulong RegisterTimer(ulong delayInMilliSeconds, TimeoutCallback callback)
        {
            return RegisterTimer(false, delayInMilliSeconds, callback, null);
        }
        
        public static ulong RegisterTimer(ulong delayInMilliSeconds, TimeoutCallback callback, object? arg)
        {
            return RegisterTimer(false, delayInMilliSeconds, callback, arg);
        }

        public static void UnregisterTimer(ulong tid)
        {
            if (_Tid2Timer.TryGetValue(tid, out var timer))
            {
                _Tid2Timer.Remove(tid);
                _Timers.Remove(timer);
            }
        }

        public static void Tick()
        {
            var now = TimeUtils.GetTimeStampInMilliseconds();
            while (true)
            {
                var timer = _Timers.Min;
                if (timer == null)
                {
                    return;
                }
                
                if (!timer.IsTimeout(now))
                {
                    break;
                }
                _Timers.Remove(timer);
                timer.OnTimeout();
                if (timer.IsRepeat)
                {
                    timer.Repeat();
                    _Timers.Add(timer);
                }
            }
        }
        

        private static readonly SortedSet<Timer> _Timers = new SortedSet<Timer>(new TimerCompare());
        private static readonly Dictionary<ulong, Timer> _Tid2Timer = new Dictionary<ulong, Timer>();

    }
}