//
// Created by xiao on 2022/10/15.
//

#include <boost/bind/bind.hpp>
#include <utility>
#include "TcpConnection.h"
#include "../Logger/Logger.h"
using namespace AsunaServer;

AsunaServer::TcpConnection::TcpConnection(
    boost::asio::io_context &io_context, 
    boost::function<void(TcpConnection*)> on_disconnect):
      socket_(io_context),
      payload_size_(0),
      payload_type_(0),
      sending_(false),
      on_receive_callback_(nullptr),
      on_send_callback_(nullptr),
      on_disconnect_callback_(std::move(on_disconnect))

{
    read_buffer_ = new unsigned char [BUFFER_SIZE];
    send_buffer_ = new unsigned char [BUFFER_SIZE];
}

TcpConnection::~TcpConnection()
{
    delete read_buffer_;
    delete send_buffer_;
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
        OnDisconnect();
        return;
    }
    if (bytes_transferred == 0)
    {
        AsunaServer::Logger::Info("eof reading header");
        OnDisconnect();
        return;
    }
    payload_size_ = *(int*)(read_buffer_);
    payload_type_ = *(unsigned int*)(read_buffer_ + 4);

    StartReadBody();
}

void TcpConnection::StartReadBody()
{
    if (payload_size_ > BUFFER_SIZE)
    {
        Logger::Error("payload size is too large.");
        OnDisconnect();
        return;
    }

    if (payload_size_ == 0)
    {
        on_receive_callback_(this, read_buffer_, payload_size_, payload_type_);
        StartReadHeader();
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
        OnDisconnect();
        return;
    }
    if (bytes_transferred == 0)
    {
        AsunaServer::Logger::Info("eof reading body");
        OnDisconnect();
        return;
    }

    on_receive_callback_(this, read_buffer_, payload_size_, payload_type_);
    StartReadHeader();
}


void TcpConnection::OnDisconnect()
{
    on_disconnect_callback_(this);
}

void TcpConnection::Send(unsigned char* data, int length, unsigned int type)
{
    if (sending_)
    {
        Logger::Error("previous send operation is not completed!");
        return;
    }

    memcpy_s(send_buffer_, 4, &length, 4);
    memcpy_s(send_buffer_ + 4, 4, &type, 4);
    memcpy_s(send_buffer_ + 8, length, data, length);
    auto buffer = boost::asio::buffer(send_buffer_, length + 8);
    auto callback = boost::bind(&TcpConnection::OnSend, this, boost::asio::placeholders::error, boost::asio::placeholders::bytes_transferred);

    sending_ = true;
    boost::asio::async_write(socket_, buffer, callback);
}

void TcpConnection::OnSend(boost::system::error_code ec, std::size_t bytes_transferred)
{
    sending_ = false;
    on_send_callback_();
}

bool TcpConnection::IsSending() const
{
    return sending_;
}

void TcpConnection::SetSendCallback(OnSendCallback on_send)
{
    on_send_callback_ = on_send;
}

void TcpConnection::SetReceiveCallback(OnReceiveCallback on_receive)
{
    on_receive_callback_ = on_receive;
}

