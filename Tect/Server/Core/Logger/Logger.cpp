//
// Created by xiao on 2022/10/15.
//

#include "Logger.h"
#include "boost/date_time/posix_time/posix_time_types.hpp"
#include <boost/date_time/posix_time/posix_time.hpp>

const char* TAG_ENGINE = "[Engine]";
const char* TAG_SCRIPT = "[Script]";

src::severity_logger<logging::trivial::severity_level> AsunaServer::Logger::logger_;

void AsunaServer::Logger::Init(const char* target, const char* fileName)
{
    InitLoggingCore();
    InitSink(target, fileName);
}

void AsunaServer::Logger::InitLoggingCore()
{
    logging::add_common_attributes();
}

void AsunaServer::Logger::InitSink(const char* target, const char* fileName)
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

void AsunaServer::Logger::Warning(const char* const message)
{
    Log(logging::trivial::severity_level::warning, message);
}

void AsunaServer::Logger::Error(const char* const message)
{
    Log(logging::trivial::severity_level::error, message);
}

void AsunaServer::Logger::Info(const char* const message)
{
    Log(logging::trivial::severity_level::info, message);
}

void AsunaServer::Logger::Debug(const char* const message)
{
    Log(logging::trivial::severity_level::debug, message);
}

void AsunaServer::Logger::Log(logging::trivial::severity_level severity, const char* message)
{
    BOOST_LOG_SEV(logger_, severity) << message;
}


