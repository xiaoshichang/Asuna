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

void Logger_Init()
{
    AsunaServer::Logger::Init();
}

void Logger_Debug(const char* message)
{
    AsunaServer::Logger::Debug(message);
}

void Logger_Info(const char* message)
{
    AsunaServer::Logger::Info(message);
}

void Logger_Warning(const char* message)
{
    AsunaServer::Logger::Warning(message);
}

void Logger_Error(const char* message)
{
    AsunaServer::Logger::Error(message);
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
