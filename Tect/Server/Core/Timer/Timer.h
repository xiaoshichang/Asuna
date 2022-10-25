//
// Created by xiao on 2022/10/16.
//

#pragma once
#include <boost/asio.hpp>
#include <unordered_map>
#include <boost/asio.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/function.hpp>

namespace XServer
{
    typedef void (* TimeoutCallback)();
    typedef unsigned int TimerID;

    class Timer
    {
    public:
        Timer(const boost::shared_ptr<boost::asio::io_context>& io_context,
              boost::posix_time::millisec delay,
              TimeoutCallback callback);

        Timer(const boost::shared_ptr<boost::asio::io_context>& io_context,
              boost::posix_time::millisec delay,
              boost::posix_time::millisec interval,
              TimeoutCallback callback);

        bool IsRepeat() const
        {
            return repeat_;
        }

        bool IsCanceled() const
        {
            return canceled_;
        }

        void Repeat();
        void Schedule(boost::function<void(boost::system::error_code)> timeout);
        void CancelTimer();
        void Timeout();

    private:
        bool repeat_;
        bool canceled_;
        boost::posix_time::millisec interval_;
        boost::function<void(void)> callback_;
        boost::asio::deadline_timer timer_;

    };
}

