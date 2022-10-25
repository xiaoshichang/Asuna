//
// Created by shichang.xiao on 2022/10/21.
//

#include "XServerAPI.h"
#include "../Server/Server.h"
#include "../Logger/Logger.h"
#include "../Timer/TimerMgr.h"

void Server_Init()
{
    XServer::Server::Init();
}

void Server_Run()
{
    XServer::Server::Run();
}

void Server_Finalize()
{
    XServer::Server::Finalize();
}

void Logger_Init()
{
    XServer::Logger::Init();
}

void Logger_Debug(const char* message)
{
    XServer::Logger::Debug(message);
}

void Logger_Info(const char* message)
{
    XServer::Logger::Info(message);
}

void Logger_Warning(const char* message)
{
    XServer::Logger::Warning(message);
}

void Logger_Error(const char* message)
{
    XServer::Logger::Error(message);
}

XServer::TimerID Timer_AddTimer(unsigned int delay, XServer::TimeoutCallback callback)
{
    return XServer::TimerMgr::AddTimer(delay, callback);
}

XServer::TimerID Timer_AddRepeatTimer(unsigned int delay, unsigned int interval, XServer::TimeoutCallback callback)
{
    return XServer::TimerMgr::AddRepeatTimer(delay, interval, callback);
}

bool Timer_CancelTimer(XServer::TimerID tid)
{
    return XServer::TimerMgr::CancelTimer(tid);
}

unsigned int Timer_GetTimersCount()
{
    return XServer::TimerMgr::GetTimersCount();
}
