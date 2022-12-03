//
// Created by xiao on 2022/10/15.
//
#pragma once
#include <set>
#include <boost/asio.hpp>
#include "TcpConnection.h"

namespace AsunaServer
{
    typedef void (*OnAcceptCallback)(TcpConnection* connection);
    typedef void (*OnConnectCallback)(TcpConnection* connection);
    typedef void (*OnDisconnectCallback)(TcpConnection* connection);

    class TcpNetwork
    {
    public:
        void InitNetwork(const boost::shared_ptr<boost::asio::io_context>& context,
                         const char* ip, int port,
                         OnAcceptCallback on_accept,
                         OnDisconnectCallback on_disconnect);
        void ConnectTo(const char* ip, int port, OnConnectCallback callback);
        void FinalizeNetwork();

    private:
        void InitAcceptor(const char* ip, int port);
        void StartAccept();
        void HandleAccept(TcpConnection* connection, const boost::system::error_code& error);
        void OnConnect(TcpConnection* connection, const boost::system::error_code& ec);
        void OnDisconnect(TcpConnection* connection);

    private:
        boost::shared_ptr<boost::asio::io_context> io_context_;
        boost::asio::ip::tcp::acceptor* acceptor_;
        std::set<TcpConnection*> connections_;

        OnAcceptCallback accept_callback_;
        OnConnectCallback connect_callback_;
        OnDisconnectCallback disconnect_callback_;
    };
}