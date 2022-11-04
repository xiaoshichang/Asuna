//
// Created by shichang.xiao on 2022/10/20.
//
#pragma once
#include <boost/asio.hpp>
#include <boost/shared_ptr.hpp>
#include "../Network/TcpNetwork.h"

namespace AsunaServer
{
    class Server
    {
    public:
        static void Init();
        static void Run();
        static void Finalize();

        static void InitInnerNetwork(const char* ip, int port,
                                     OnAcceptCallback on_accept,
                                     OnDisconnectCallback on_disconnect);
        static void ConnectTo(const char* ip, int port, OnDisconnectCallback on_connect);
        static void FinalizeInnerNetwork();

        static void InitOuterNetwork(const char* ip, int port,
                                     OnAcceptCallback on_accept,
                                     OnDisconnectCallback on_disconnect);
        static void FinalizeOuterNetwork();

    private:
        static void InitTimerManager();
        static void FinalizeTimerManager();



    private:
        static boost::shared_ptr<boost::asio::io_context> io_context_;
        static boost::shared_ptr<AsunaServer::TcpNetwork> inner_network_;
        static boost::shared_ptr<AsunaServer::TcpNetwork> outer_network_;
    };
}
