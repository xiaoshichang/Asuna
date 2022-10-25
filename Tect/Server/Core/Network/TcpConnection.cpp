//
// Created by xiao on 2022/10/15.
//

#include "TcpConnection.h"
using namespace XServer;

XServer::TcpConnection::TcpConnection(boost::asio::io_context &io_context)
    : socket_(io_context)
{

}

std::shared_ptr<TcpConnection> XServer::TcpConnection::Create(boost::asio::io_context &io_context)
{
    return std::make_shared<TcpConnection>(io_context);
}

void TcpConnection::Start()
{

}
