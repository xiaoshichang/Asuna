//
// Created by xiao on 2022/10/15.
//

#pragma once

#include <boost/asio.hpp>
#include <boost/enable_shared_from_this.hpp>

using boost::asio::ip::tcp;

namespace AsunaServer
{
    class TcpConnection
    {

    public:

        explicit TcpConnection(boost::asio::io_context& io_context);
        ~TcpConnection();

        tcp::socket& socket()
        {
            return socket_;
        }

        void Start();
        void StartReadHeader();
        void StartReadBody();
        void HandleReadHeader(boost::system::error_code ec, std::size_t bytes_transferred);
        void HandleReadBody(boost::system::error_code ec, std::size_t bytes_transferred);

        void Disconnect();

    private:
        tcp::socket socket_;
        unsigned int payload_size_;
        unsigned int payload_type_;
        unsigned char* read_buffer_;

        const int HEADER_SIZE = 8;
        const int BUFFER_SIZE = 2048;
    };
}



