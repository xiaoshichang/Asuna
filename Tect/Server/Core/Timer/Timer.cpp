//
// Created by xiao on 2022/10/16.
//

#include "Timer.h"
#include "../Logger/Logger.h"

AsunaServer::Timer::Timer(const boost::shared_ptr<boost::asio::io_context> &io_context,
                          boost::posix_time::millisec delay,
                          TimeoutCallback callback):
      repeat_(false),
      canceled_(false),
      interval_(boost::posix_time::millisec(0)),
      callback_(callback),
      timer_(*io_context, delay)
{
}

AsunaServer::Timer::Timer(const boost::shared_ptr<boost::asio::io_context> &io_context,
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

void AsunaServer::Timer::Repeat()
{
    auto expires = timer_.expires_at();
    auto next = expires + interval_;
    timer_.expires_at(next);
}

void AsunaServer::Timer::Schedule(boost::function<void(boost::system::error_code)> timeout)
{
    timer_.async_wait(timeout);
}

void AsunaServer::Timer::CancelTimer()
{
    canceled_ = true;
    timer_.cancel();
}

void AsunaServer::Timer::Timeout()
{
    callback_();
}





