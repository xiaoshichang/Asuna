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
                             AcceptCallback on_accept,
                             DisconnectCallback on_disconnect,
                             ReceiveCallback on_receive)
{
    io_context_ = context;
    accept_callback_ = on_accept;
    disconnect_callback_ = on_disconnect;
    receive_callback_ = on_receive;

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
    auto connection = new TcpConnection(*io_context_);
    auto callback = boost::bind(&TcpNetwork::HandleAccept, this, connection, boost::asio::placeholders::error);
    acceptor_->async_accept(connection->socket(), callback);
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

void TcpNetwork::OnDisconnectFromRemote(TcpConnection *connection)
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

