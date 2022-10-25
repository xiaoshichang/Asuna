//
// Created by xiao on 2022/10/22.
//
#pragma once
#include <boost/asio.hpp>
#include <unordered_map>
#include <boost/asio.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <boost/shared_ptr.hpp>
#include "Timer.h"


namespace AsunaServer
{

    class TimerMgr
    {

    public:
        static void Init(const boost::shared_ptr<boost::asio::io_context>& io_context);
        static void Finalize();
        static TimerID AllocateTimerID();
        static TimerID AddTimer(unsigned int delay_ms, TimeoutCallback callback);
        static TimerID AddRepeatTimer(unsigned int delay_ms, unsigned int interval_ms, TimeoutCallback callback);
        static bool CancelTimer(TimerID tid);
        static void OnTimeout(const boost::shared_ptr<Timer>& timer, TimerID tid, const boost::system::error_code& error);
        static unsigned int GetTimersCount();

    private:
        static TimerID global_timer_id;
        static boost::shared_ptr<boost::asio::io_context> io_context_;
        static std::unordered_map<TimerID, boost::shared_ptr<Timer>> timers_;

    };
}


