//
// Created by xiao on 2022/10/15.
//
#pragma once
#include <set>
#include <boost/asio.hpp>
#include "TcpConnection.h"

namespace AsunaServer
{
    class TcpNetwork
    {
    public:
        void InitNetwork(const boost::shared_ptr<boost::asio::io_context>& context, const char* ip, int port);
        void FinalizeNetwork();

    private:
        void InitAcceptor(const char* ip, int port);
        void StartAccept();
        void HandleAccept(const std::shared_ptr<TcpConnection>& connection, const boost::system::error_code& error);

    private:
        boost::shared_ptr<boost::asio::io_context> io_context_;
        boost::asio::ip::tcp::acceptor* acceptor_;
        std::set<std::shared_ptr<TcpConnection>> connections_;

    };
}