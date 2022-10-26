//
// Created by xiao on 2022/10/15.
//
#pragma once
#include <set>
#include <boost/asio.hpp>
#include "TcpConnection.h"

namespace AsunaServer
{
    typedef void (*AcceptCallback)(TcpConnection* connection);
    typedef void (*DisconnectCallback)(TcpConnection* connection);
    typedef void (*ReceiveCallback)(TcpConnection* connection, unsigned char *data, int length);

    class TcpNetwork
    {
    public:
        void InitNetwork(const boost::shared_ptr<boost::asio::io_context>& context,
                         const char* ip, int port,
                         AcceptCallback on_accept,
                         DisconnectCallback on_disconnect,
                         ReceiveCallback on_receive);
        void FinalizeNetwork();

    private:
        void InitAcceptor(const char* ip, int port);
        void StartAccept();
        void HandleAccept(TcpConnection* connection, const boost::system::error_code& error);

        void Disconnect(TcpConnection* connection);
        void OnDisconnectFromRemote(TcpConnection* connection);

    private:
        boost::shared_ptr<boost::asio::io_context> io_context_;
        boost::asio::ip::tcp::acceptor* acceptor_;
        std::set<TcpConnection*> connections_;

        AcceptCallback accept_callback_;
        DisconnectCallback disconnect_callback_;
        ReceiveCallback receive_callback_;


    };
}