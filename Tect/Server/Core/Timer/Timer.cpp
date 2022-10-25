//
// Created by xiao on 2022/10/16.
//

#include "Timer.h"
#include "../Logger/Logger.h"

XServer::Timer::Timer(const boost::shared_ptr<boost::asio::io_context> &io_context,
                      boost::posix_time::millisec delay,
                      TimeoutCallback callback):
      repeat_(false),
      canceled_(false),
      interval_(boost::posix_time::millisec(0)),
      callback_(callback),
      timer_(*io_context, delay)
{
}

XServer::Timer::Timer(const boost::shared_ptr<boost::asio::io_context> &io_context,
                      boost::posix_time::millisec delay,
                      boost::posix_time::millisec interval,
                      TimeoutCallback callback):
        repeat_(true),
        canceled_(false),
        interval_(interval),
        callback_(callback),
        timer_(*io_context, delay)
{
}

void XServer::Timer::Repeat()
{
    auto expires = timer_.expires_at();
    auto next = expires + interval_;
    timer_.expires_at(next);
}

void XServer::Timer::Schedule(boost::function<void(boost::system::error_code)> timeout)
{
    timer_.async_wait(timeout);
}

void XServer::Timer::CancelTimer()
{
    canceled_ = true;
    timer_.cancel();
}

void XServer::Timer::Timeout()
{
    callback_();
}





