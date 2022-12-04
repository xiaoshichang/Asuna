//
// Created by xiao on 2022/10/15.
//

#pragma once

#include <boost/asio.hpp>
#include <boost/function.hpp>

using boost::asio::ip::tcp;

namespace AsunaServer
{
    class TcpConnection;
    typedef void (*OnReceiveCallback)(TcpConnection* connection, unsigned char *payload_data, int payload_size, unsigned int payload_type);
    typedef void (*OnSendCallback)();

    class TcpConnection
    {

    public:

        explicit TcpConnection(boost::asio::io_context& io_context,
                               boost::function<void(TcpConnection*)> on_disconnect);
        ~TcpConnection();

        tcp::socket& socket()
        {
            return socket_;
        }

        void SetReceiveCallback(OnReceiveCallback on_receive);
        void SetSendCallback(OnSendCallback);

        void Start();
        void StartReadHeader();
        void StartReadBody();
        void HandleReadHeader(boost::system::error_code ec, std::size_t bytes_transferred);
        void HandleReadBody(boost::system::error_code ec, std::size_t bytes_transferred);

        void OnDisconnect();

        void Send(unsigned char* data, int length, unsigned int type);
        void OnSend(boost::system::error_code ec, std::size_t bytes_transferred);
        bool IsSending() const;

    private:

        tcp::socket socket_;
        unsigned int payload_size_;
        unsigned int payload_type_;
        unsigned char* read_buffer_;
        unsigned char* send_buffer_;
        bool sending_;

        OnReceiveCallback on_receive_callback_;
        OnSendCallback on_send_callback_;
        boost::function<void(TcpConnection*)> on_disconnect_callback_;

        const int HEADER_SIZE = 8;
        const int BUFFER_SIZE = 4096;

    };
}



