//
// Created by shichang.xiao on 2022/10/21.
//

#pragma once

#ifdef _WIN32
#  define AsunaServerAPI __declspec( dllexport )
#else
#  define XServerAPI
#endif


#include "../Timer/TimerMgr.h"

#ifdef __cplusplus
extern "C"
{
#endif

AsunaServerAPI void Server_Init();
AsunaServerAPI void Server_Run();
AsunaServerAPI void Server_Finalize();


AsunaServerAPI void Logger_Debug(const char* message);
AsunaServerAPI void Logger_Info(const char* message);
AsunaServerAPI void Logger_Warning(const char* message);
AsunaServerAPI void Logger_Error(const char* message);


AsunaServerAPI AsunaServer::TimerID Timer_AddTimer(unsigned int delay, AsunaServer::TimeoutCallback callback);
AsunaServerAPI AsunaServer::TimerID Timer_AddRepeatTimer(unsigned int delay, unsigned int interval, AsunaServer::TimeoutCallback callback);
AsunaServerAPI bool Timer_CancelTimer(AsunaServer::TimerID);
AsunaServerAPI unsigned int Timer_GetTimersCount();


#ifdef __cplusplus
}
#endif