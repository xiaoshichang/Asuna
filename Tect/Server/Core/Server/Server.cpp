//
// Created by shichang.xiao on 2022/10/20.
//

#include "Server.h"
#include "../Logger/Logger.h"
#include "../Network/TcpNetwork.h"
#include "../Timer/TimerMgr.h"

using namespace AsunaServer;

boost::shared_ptr<boost::asio::io_context> Server::io_context_;
boost::shared_ptr<AsunaServer::TcpNetwork> Server::inner_network_;
boost::shared_ptr<AsunaServer::TcpNetwork> Server::outer_network_;


void Server::Init()
{
    io_context_ = boost::shared_ptr<boost::asio::io_context>(new boost::asio::io_context());
    InitLogger();
    InitTimerManager();
}

void Server::Finalize()
{
    FinalizeTimerManager();
}

void Server::Run()
{
    Logger::Info("Running ...");
    io_context_->run();
}

void Server::InitLogger()
{
    Logger::Init();
}

void Server::InitTimerManager()
{
    Logger::Info("InitTimerManager");
    TimerMgr::Init(io_context_);
}

void Server::FinalizeTimerManager()
{
    Logger::Info("FinalizeTimerManager");
    TimerMgr::Finalize();
}

void Server::InitInnerNetwork(const char* ip, int port,
                              OnAcceptCallback on_accept,
                              OnDisconnectCallback on_disconnect,
                              OnReceiveCallback on_receive)
{
    Logger::Info("InitInnerNetwork");
    inner_network_ = boost::make_shared<TcpNetwork>();
    inner_network_->InitNetwork(io_context_, ip, port, on_accept, on_disconnect, on_receive);
}

void Server::FinalizeInnerNetwork()
{
    inner_network_->FinalizeNetwork();
}

void Server::InitOuterNetwork(const char* ip, int port,
                              OnAcceptCallback on_accept,
                              OnDisconnectCallback on_disconnect,
                              OnReceiveCallback on_receive)
{
    Logger::Info("InitOuterNetwork");
    outer_network_ = boost::make_shared<TcpNetwork>();
    outer_network_->InitNetwork(io_context_, ip, port, on_accept, on_disconnect, on_receive);
}

void Server::FinalizeOuterNetwork()
{
    outer_network_->FinalizeNetwork();
}

void Server::SendInner(TcpConnection *connection, unsigned char* data, unsigned int length)
{

}

void Server::SendOuter(TcpConnection *connection, unsigned char* data, unsigned int length)
{

}


