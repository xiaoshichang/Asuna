//
// Created by xiao on 2022/10/15.
//

#include <boost/bind/bind.hpp>
#include "TcpConnection.h"
#include "../Logger/Logger.h"
using namespace AsunaServer;

AsunaServer::TcpConnection::TcpConnection(boost::asio::io_context &io_context)
    : socket_(io_context)
{
    read_buffer_ = new unsigned char [BUFFER_SIZE];
}

TcpConnection::~TcpConnection()
{
    delete read_buffer_;
}

void TcpConnection::Start()
{
    StartReadHeader();
}

void TcpConnection::StartReadHeader()
{
    auto buffer = boost::asio::buffer(read_buffer_, HEADER_SIZE);
    auto callback = boost::bind(&TcpConnection::HandleReadHeader, this, boost::asio::placeholders::error, boost::asio::placeholders::bytes_transferred);
    boost::asio::async_read(socket_, buffer, callback);
}

void TcpConnection::HandleReadHeader(boost::system::error_code ec, std::size_t bytes_transferred)
{
    if (ec)
    {
        AsunaServer::Logger::Info("error reading header");
        return;
    }
    if (bytes_transferred == 0)
    {
        AsunaServer::Logger::Info("eof reading header");
        return;
    }
    payload_size_ = (unsigned int)*read_buffer_;
    payload_type_ = (unsigned int)*(read_buffer_ + 4);
    StartReadBody();
}

void TcpConnection::StartReadBody()
{
    if (payload_size_ > BUFFER_SIZE)
    {
        return;
    }

    auto buffer = boost::asio::buffer(read_buffer_, payload_size_);
    auto callback = boost::bind(&TcpConnection::HandleReadBody, this, boost::asio::placeholders::error, boost::asio::placeholders::bytes_transferred);
    boost::asio::async_read(socket_, buffer, callback);
}

void TcpConnection::HandleReadBody(boost::system::error_code ec, std::size_t bytes_transferred)
{
    if (ec)
    {
        AsunaServer::Logger::Info("error reading body");
        return;
    }
    if (bytes_transferred == 0)
    {
        AsunaServer::Logger::Info("eof reading body");
        return;
    }


}

void TcpConnection::Disconnect()
{
    socket_.shutdown(boost::asio::socket_base::shutdown_both);
}

