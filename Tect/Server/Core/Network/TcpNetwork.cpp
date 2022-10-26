//
// Created by xiao on 2022/10/15.
//

#include <boost/bind/bind.hpp>
#include "../Logger/Logger.h"
#include "TcpNetwork.h"

using boost::asio::ip::tcp;
using namespace AsunaServer;



void TcpNetwork::InitNetwork(const boost::shared_ptr<boost::asio::io_context>& context, const char* ip, int port)
{
    io_context_ = context;
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
    auto connection = TcpConnection::Create(*io_context_);
    auto callback = boost::bind(&TcpNetwork::HandleAccept, this, connection, boost::asio::placeholders::error);
    acceptor_->async_accept(connection->socket(), callback);
}

void TcpNetwork::HandleAccept(const std::shared_ptr<TcpConnection>& connection, const boost::system::error_code& error)
{
    if (!error)
    {
        Logger::Info("HandleAccept");
        connections_.insert(connection);
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

