//
// Created by xiao on 2022/10/15.
//

#include "TcpConnection.h"
using namespace AsunaServer;

AsunaServer::TcpConnection::TcpConnection(boost::asio::io_context &io_context)
    : socket_(io_context)
{

}

std::shared_ptr<TcpConnection> AsunaServer::TcpConnection::Create(boost::asio::io_context &io_context)
{
    return std::make_shared<TcpConnection>(io_context);
}

void TcpConnection::Start()
{

}
