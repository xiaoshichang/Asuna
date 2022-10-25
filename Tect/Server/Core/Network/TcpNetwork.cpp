//
// Created by xiao on 2022/10/15.
//

#include <boost/bind/bind.hpp>
#include "../Logger/Logger.h"
#include "TcpNetwork.h"

using boost::asio::ip::tcp;
using namespace AsunaServer;

boost::shared_ptr<boost::asio::io_context> TcpNetwork::io_context_;
tcp::acceptor* TcpNetwork::acceptor_ = nullptr;
std::set<std::shared_ptr<TcpConnection>> TcpNetwork::connections_;



void TcpNetwork::InitNetwork(const boost::shared_ptr<boost::asio::io_context>& context)
{
    io_context_ = context;
    InitAcceptor();
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
    auto callback = boost::bind(&TcpNetwork::HandleAccept, connection, boost::asio::placeholders::error);
    acceptor_->async_accept(connection->socket(), callback);
}

void TcpNetwork::HandleAccept(std::shared_ptr<TcpConnection> connection, const boost::system::error_code& error)
{
    if (!error)
    {
        Logger::Info("HandleAccept");
        connections_.insert(connection);
        connection->Start();
    }
    StartAccept();
}

void TcpNetwork::InitAcceptor()
{
    auto address = boost::asio::ip::address_v4::from_string("0.0.0.0");
    auto endpoint = tcp::endpoint(address, 40001);
    acceptor_ = new tcp::acceptor(*io_context_, endpoint);
}

