cmake_minimum_required(VERSION 3.20)
project(XServer)

set(CMAKE_CXX_STANDARD 20)

if (WIN32)
    set(BOOST_ROOT C:\\Boost)
else()
endif()

find_package(Boost COMPONENTS log REQUIRED)

add_library(XServer MODULE
        Network/TcpNetwork.cpp
        Network/TcpNetwork.h
        Network/TcpConnection.cpp
        Network/TcpConnection.h
        Logger/Logger.cpp
        Logger/Logger.h
        Server/Server.cpp
        Server/Server.h
        Timer/Timer.cpp
        Timer/Timer.h
        Timer/TimerMgr.cpp
        Timer/TimerMgr.h
        ModuleExport/AsunaServerAPI.cpp
        ModuleExport/AsunaServerAPI.h
        )

target_link_directories(XServer PRIVATE  ${Boost_LIBRARY_DIRS})

if (WIN32)
    target_link_libraries(XServer PRIVATE ws2_32 wsock32 ${Boost_LIBRARIES})
else()
    target_link_libraries(XServer PRIVATE ${Boost_LIBRARIES})
endif()

target_include_directories(XServer PRIVATE ${Boost_INCLUDE_DIRS})
