//
// Created by shichang.xiao on 2022/10/21.
//

#include "CoreAPI.h"
#include "../Server/Server.h"
#include "../Logger/Logger.h"
#include "../Timer/TimerMgr.h"

void Server_Init()
{
    AsunaServer::Server::Init();
}

void Server_Run()
{
    AsunaServer::Server::Run();
}

void Server_Finalize()
{
    AsunaServer::Server::Finalize();
}

void Logger_Init(const char* target, const char* fileName)
{
    AsunaServer::Logger::Init(target, fileName);
}

void Logger_Debug(const char* message)
{
    AsunaServer::Logger::Debug(message, LogTag::Managed);
}

void Logger_Info(const char* message)
{
    AsunaServer::Logger::Info(message, LogTag::Managed);
}

void Logger_Warning(const char* message)
{
    AsunaServer::Logger::Warning(message, LogTag::Managed);
}

void Logger_Error(const char* message)
{
    AsunaServer::Logger::Error(message, LogTag::Managed);
}

AsunaServer::TimerID Timer_AddTimer(unsigned int delay, AsunaServer::TimeoutCallback callback)
{
    return AsunaServer::TimerMgr::AddTimer(delay, callback);
}

AsunaServer::TimerID Timer_AddRepeatTimer(unsigned int delay, unsigned int interval, AsunaServer::TimeoutCallback callback)
{
    return AsunaServer::TimerMgr::AddRepeatTimer(delay, interval, callback);
}

bool Timer_CancelTimer(AsunaServer::TimerID tid)
{
    return AsunaServer::TimerMgr::CancelTimer(tid);
}

unsigned int Timer_GetTimersCount()
{
    return AsunaServer::TimerMgr::GetTimersCount();
}

void InnerNetwork_Init(const char* ip, int port,
                       OnAcceptCallback on_accept,
                       OnDisconnectCallback on_disconnect)
{
    AsunaServer::Server::InitInnerNetwork(ip, port, on_accept, on_disconnect);
}

void InnerNetwork_Send(TcpConnection* connection, unsigned char* data, unsigned int length, unsigned int type)
{
    connection->Send(data, length, type);
}

void InnerNetwork_Finalize()
{
    AsunaServer::Server::FinalizeInnerNetwork();
}

void OuterNetwork_Init(const char* ip, int port,
                       OnAcceptCallback on_accept,
                       OnDisconnectCallback on_disconnect)
{
    AsunaServer::Server::InitOuterNetwork(ip, port, on_accept, on_disconnect);
}

void OuterNetwork_Send(TcpConnection* connection, unsigned char* data, unsigned int length, unsigned int type)
{
    connection->Send(data, length, type);
}

void OuterNetwork_Finalize()
{
    AsunaServer::Server::FinalizeOuterNetwork();
}

void Connection_SetReceiveCallback(TcpConnection* connection, OnReceiveCallback on_receive)
{
    connection->SetReceiveCallback(on_receive);
}

void Connection_SetSendCallback(TcpConnection* connection, OnSendCallback on_send)
{
    connection->SetSendCallback(on_send);
}

bool Connection_IsSending(TcpConnection* connection)
{
    return connection->IsSending();
}