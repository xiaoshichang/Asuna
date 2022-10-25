//
// Created by shichang.xiao on 2022/10/20.
//
#pragma once
#include <boost/asio.hpp>
#include <boost/shared_ptr.hpp>

namespace AsunaServer
{
    class Server
    {
    public:
        static void Init();
        static void Run();
        static void Finalize();

    private:
        static void InitLogger();
        static void InitNetwork();
        static void FinalizeNetwork();
        static void InitTimerManager();
        static void FinalizeTimerManager();

    private:
        static boost::shared_ptr<boost::asio::io_context> io_context_;
    };
}
