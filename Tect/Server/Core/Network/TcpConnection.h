//
// Created by xiao on 2022/10/15.
//

#pragma once

#include <boost/asio.hpp>
#include <boost/enable_shared_from_this.hpp>

using boost::asio::ip::tcp;

namespace AsunaServer
{
    class TcpConnection : boost::enable_shared_from_this<TcpConnection>
    {

    public:

        explicit TcpConnection(boost::asio::io_context& io_context);
        tcp::socket& socket()
        {
            return socket_;
        }

        void Start();
        static std::shared_ptr<TcpConnection> Create(boost::asio::io_context& io_context);

    private:
        tcp::socket socket_;
    };
}



