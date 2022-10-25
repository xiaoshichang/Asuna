//
// Created by shichang.xiao on 2022/10/20.
//

#include "Server.h"
#include "../Logger/Logger.h"
#include "../Network/TcpNetwork.h"
#include "../Timer/TimerMgr.h"

using namespace XServer;

boost::shared_ptr<boost::asio::io_context> Server::io_context_;

void Server::Init()
{
    io_context_ = boost::shared_ptr<boost::asio::io_context>(new boost::asio::io_context());
    InitLogger();
    InitTimerManager();
    InitNetwork();
}

void Server::Finalize()
{
    FinalizeNetwork();
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

void Server::InitNetwork()
{
    Logger::Info("InitNetwork");
    TcpNetwork::InitNetwork(io_context_);
}

void Server::FinalizeNetwork()
{
    Logger::Info("FinalizeNetwork");
    TcpNetwork::FinalizeNetwork();
}


