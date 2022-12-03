//
// Created by xiao on 2022/10/15.
//

#include <boost/bind/bind.hpp>
#include "../Logger/Logger.h"
#include "TcpNetwork.h"

using boost::asio::ip::tcp;
using namespace AsunaServer;



void TcpNetwork::InitNetwork(const boost::shared_ptr<boost::asio::io_context>& context,
                             const char* ip,
                             int port,
                             OnAcceptCallback on_accept,
                             OnDisconnectCallback on_disconnect)
{
    io_context_ = context;
    accept_callback_ = on_accept;
    disconnect_callback_ = on_disconnect;

    InitAcceptor(ip, port);
    StartAccept();
}

void TcpNetwork::FinalizeNetwork()
{
    // todo: shutdown network gracefully
    delete acceptor_;
    io_context_ = nullptr;
}

void TcpNetwork::StartAccept()
{
    auto on_disconnect = boost::bind(&TcpNetwork::OnDisconnect, this,boost::placeholders::_1);
    auto connection = new TcpConnection(*io_context_,  on_disconnect);

    auto on_accept = boost::bind(&TcpNetwork::HandleAccept, this, connection, boost::asio::placeholders::error);
    acceptor_->async_accept(connection->socket(), on_accept);
}

void TcpNetwork::HandleAccept(TcpConnection* connection, const boost::system::error_code& error)
{
    if (!error)
    {
        connections_.insert(connection);
        accept_callback_(connection);
        connection->Start();
    }
    StartAccept();
}

void TcpNetwork::InitAcceptor(const char* ip, int port)
{
    auto address = boost::asio::ip::address_v4::from_string(ip);
    auto endpoint = tcp::endpoint(address, port);
    acceptor_ = new tcp::acceptor(*io_context_, endpoint);
}

void TcpNetwork::OnDisconnect(TcpConnection *connection)
{
    if (disconnect_callback_ != nullptr)
    {
        disconnect_callback_(connection);
    }
    connections_.erase(connection);
    delete connection;
}

void TcpNetwork::ConnectTo(const char *ip, int port, OnConnectCallback callback)
{
    connect_callback_ = callback;
    auto connection = new TcpConnection(*io_context_, boost::bind(&TcpNetwork::OnDisconnect, this, boost::placeholders::_1));
    auto address = boost::asio::ip::address_v4::from_string(ip);
    auto endpoint = tcp::endpoint(address, port);
    connection->socket().async_connect(endpoint, boost::bind(&TcpNetwork::OnConnect, this, connection, boost::asio::placeholders::error));
}

void TcpNetwork::OnConnect(TcpConnection* connection, const boost::system::error_code &ec)
{
    if (!ec)
    {
        connections_.insert(connection);
        connection->Start();
    }
    connect_callback_(connection);
}
