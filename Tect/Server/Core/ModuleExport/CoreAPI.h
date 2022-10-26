//
// Created by shichang.xiao on 2022/10/21.
//

#pragma once

#ifdef _WIN32
#  define XServerAPI __declspec( dllexport )
#else
#  define XServerAPI
#endif


#include "../Timer/TimerMgr.h"

#ifdef __cplusplus
extern "C"
{
#endif

XServerAPI void Server_Init();
XServerAPI void Server_Run();
XServerAPI void Server_Finalize();


XServerAPI void Logger_Debug(const char* message);
XServerAPI void Logger_Info(const char* message);
XServerAPI void Logger_Warning(const char* message);
XServerAPI void Logger_Error(const char* message);


XServerAPI AsunaServer::TimerID Timer_AddTimer(unsigned int delay, AsunaServer::TimeoutCallback callback);
XServerAPI AsunaServer::TimerID Timer_AddRepeatTimer(unsigned int delay, unsigned int interval, AsunaServer::TimeoutCallback callback);
XServerAPI bool Timer_CancelTimer(AsunaServer::TimerID);
XServerAPI unsigned int Timer_GetTimersCount();


#ifdef __cplusplus
}
#endif