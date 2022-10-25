//
// Created by xiao on 2022/10/15.
//

#include "Logger.h"
#include "boost/date_time/posix_time/posix_time_types.hpp"
#include <boost/date_time/posix_time/posix_time.hpp>

const char* TAG_ENGINE = "[Engine]";
const char* TAG_SCRIPT = "[Script]";

src::severity_logger<logging::trivial::severity_level> XServer::Logger::logger_;

void XServer::Logger::Init()
{
    InitLoggingCore();
    InitSink();
}

void XServer::Logger::InitLoggingCore()
{
    logging::add_common_attributes();
}

void XServer::Logger::InitSink()
{
    // formatter
    logging::formatter formatter = expr::format("[%1%][%2%] - %3%")
        % expr::format_date_time< boost::posix_time::ptime >("TimeStamp", "%Y-%m-%d %H:%M:%S")
        % logging::trivial::severity
        % expr::message;

    // console
    auto consoleSink = logging::add_console_log();
    consoleSink->set_formatter(formatter);
    consoleSink->set_filter(logging::trivial::severity >= logging::trivial::debug);

    // file
    auto fileSink = logging::add_file_log(
        keywords::file_name = "sample_%N.log",
        keywords::rotation_size = 10 * 1024 * 1024
    );

    fileSink->set_formatter(formatter);
    fileSink->set_filter(logging::trivial::severity >= logging::trivial::info);
}

void XServer::Logger::Finalize()
{

}

void XServer::Logger::Warning(const char* const message)
{
    Log(logging::trivial::severity_level::warning, message);
}

void XServer::Logger::Error(const char* const message)
{
    Log(logging::trivial::severity_level::error, message);
}

void XServer::Logger::Info(const char* const message)
{
    Log(logging::trivial::severity_level::info, message);
}

void XServer::Logger::Debug(const char* const message)
{
    Log(logging::trivial::severity_level::debug, message);
}

void XServer::Logger::Log(logging::trivial::severity_level severity, const char* message)
{
    BOOST_LOG_SEV(logger_, severity) << message;
}


