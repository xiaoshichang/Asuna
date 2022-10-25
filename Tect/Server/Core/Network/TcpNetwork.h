//
// Created by xiao on 2022/10/15.
//
#pragma once
#include <set>
#include <boost/asio.hpp>
#include "TcpConnection.h"

namespace XServer
{
    class TcpNetwork
    {
    public:
        static void InitNetwork(const boost::shared_ptr<boost::asio::io_context>& context);
        static void FinalizeNetwork();

    private:
        void static InitAcceptor();
        void static StartAccept();
        void static HandleAccept(std::shared_ptr<TcpConnection> connection, const boost::system::error_code& error);

    private:
        static boost::shared_ptr<boost::asio::io_context> io_context_;
        static boost::asio::ip::tcp::acceptor* acceptor_;
        static std::set<std::shared_ptr<TcpConnection>> connections_;

    };
}