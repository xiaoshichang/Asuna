//
// Created by xiao on 2022/10/15.
//

#include "Logger.h"
#include "boost/date_time/posix_time/posix_time_types.hpp"
#include <boost/date_time/posix_time/posix_time.hpp>
#include <boost/log/attributes/constant.hpp>


src::severity_logger<logging::trivial::severity_level> AsunaServer::Logger::core_logger_;
src::severity_logger<logging::trivial::severity_level> AsunaServer::Logger::managed_logger_;

void AsunaServer::Logger::Init(const char* target, const char* fileName)
{
    InitLoggingCore();
    InitSink(target, fileName);
}

void AsunaServer::Logger::InitLoggingCore()
{
    logging::add_common_attributes();
    core_logger_.add_attribute("Tag", attrs::constant<std::string>("Core"));
    managed_logger_.add_attribute("Tag", attrs::constant<std::string>("Managed"));
}

void AsunaServer::Logger::InitSink(const char* target, const char* fileName)
{
    // formatter
    logging::formatter formatter = expr::format("[%1%][%2%][%3%] - %4%")
        % expr::format_date_time< boost::posix_time::ptime >("TimeStamp", "%Y-%m-%d %H:%M:%S")
        % logging::trivial::severity
        % expr::attr<std::string>("Tag")
        % expr::message;

    // console
    auto consoleSink = logging::add_console_log();
    consoleSink->set_formatter(formatter);
    consoleSink->set_filter(logging::trivial::severity >= logging::trivial::debug);

    // file
    auto fileSink = logging::add_file_log(
        keywords::open_mode = std::ios_base::app,
        keywords::target = target,
        keywords::file_name = fileName
    );

    fileSink->set_formatter(formatter);
    fileSink->set_filter(logging::trivial::severity >= logging::trivial::debug);
    fileSink->locked_backend()->auto_flush();
}

void AsunaServer::Logger::Finalize()
{

}

void AsunaServer::Logger::Warning(const char* const message, LogTag tag)
{
    Log(logging::trivial::severity_level::warning, message, tag);
}

void AsunaServer::Logger::Error(const char* const message, LogTag tag)
{
    Log(logging::trivial::severity_level::error, message, tag);
}

void AsunaServer::Logger::Info(const char* const message, LogTag tag)
{
    Log(logging::trivial::severity_level::info, message, tag);
}

void AsunaServer::Logger::Debug(const char* const message, LogTag tag)
{
    Log(logging::trivial::severity_level::debug, message, tag);
}

void AsunaServer::Logger::Log(logging::trivial::severity_level severity, const char* message, LogTag tag)
{
    if (tag == LogTag::Core)
    {
        BOOST_LOG_SEV(core_logger_, severity) << message;
    }
    else
    {
        BOOST_LOG_SEV(managed_logger_, severity) << message;
    }
}


