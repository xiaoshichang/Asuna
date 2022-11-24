cmake_minimum_required(VERSION 3.20)
project(AsunaServer)

set(CMAKE_CXX_STANDARD 20)

if (WIN32)
    set(BOOST_ROOT C:\\Boost)
else()
endif()

find_package(Boost COMPONENTS log REQUIRED)

add_library(AsunaServer MODULE
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
        ModuleExport/CoreAPI.cpp
        ModuleExport/CoreAPI.h
        )

target_link_directories(AsunaServer PRIVATE  ${Boost_LIBRARY_DIRS})

if (WIN32)
    target_link_libraries(AsunaServer PRIVATE ws2_32 wsock32 ${Boost_LIBRARIES})
else()
    target_link_libraries(AsunaServer PRIVATE ${Boost_LIBRARIES})
endif()

target_include_directories(AsunaServer PRIVATE ${Boost_INCLUDE_DIRS})
install(TARGETS AsunaServer DESTINATION ${CMAKE_SOURCE_DIR}/../Bin)