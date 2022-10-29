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
#include "../Network/TcpNetwork.h"

using namespace AsunaServer;

#ifdef __cplusplus
extern "C"
{
#endif

XServerAPI void Server_Init();
XServerAPI void Server_Run();
XServerAPI void Server_Finalize();

XServerAPI void Logger_Init(const char* target, const char* fileName);
XServerAPI void Logger_Debug(const char* message);
XServerAPI void Logger_Info(const char* message);
XServerAPI void Logger_Warning(const char* message);
XServerAPI void Logger_Error(const char* message);


XServerAPI TimerID Timer_AddTimer(unsigned int delay, TimeoutCallback callback);
XServerAPI TimerID Timer_AddRepeatTimer(unsigned int delay, unsigned int interval, TimeoutCallback callback);
XServerAPI bool Timer_CancelTimer(TimerID);
XServerAPI unsigned int Timer_GetTimersCount();


XServerAPI void InnerNetwork_Init(const char* ip, int port,
                                  OnAcceptCallback on_accept,
                                  OnDisconnectCallback on_disconnect,
                                  OnReceiveCallback on_receive);
XServerAPI void InnerNetwork_Send(TcpConnection* connection, unsigned char* data, unsigned int length);
XServerAPI void InnerNetwork_Finalize();

XServerAPI void OuterNetwork_Init(const char* ip, int port,
                                  OnAcceptCallback on_accept,
                                  OnDisconnectCallback on_disconnect,
                                  OnReceiveCallback on_receive);
XServerAPI void OuterNetwork_Send(TcpConnection* connection, unsigned char* data, unsigned int length);
XServerAPI void OuterNetwork_Finalize();

#ifdef __cplusplus
}
#endif