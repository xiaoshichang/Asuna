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
    auto on_disconnect = boost::bind(&TcpNetwork::OnDisconnect, this,
                                     boost::placeholders::_1);
    auto connection = new TcpConnection(*io_context_,  on_disconnect);

    auto on_accept = boost::bind(&TcpNetwork::HandleAccept, this, connection, boost::asio::placeholders::error);
    acceptor_->async_accept(connection->socket(), on_accept);
}

void TcpNetwork::HandleAccept(TcpConnection* connection, const boost::system::error_code& error)
{
    if (!error)
    {
        Logger::Info("HandleAccept");
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
    connections_.erase(connection);
    delete connection;
}

void TcpNetwork::Disconnect(TcpConnection *connection)
{
    connection->Disconnect();
    connections_.erase(connection);
    delete connection;
}
