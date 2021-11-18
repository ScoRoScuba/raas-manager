using System;
using OFX.RAASManager.Core.Interfaces;
using Serilog;

namespace OFX.RAASManager.Core
{
    public class LoggerService : ILoggerService
    {
        private ILogger _serilogLogger;

        public LoggerService(ILogger serilogLogger)
        {
            _serilogLogger = serilogLogger;
        }

        public void Debug(string message, Exception exception)
        {
            _serilogLogger.Debug(exception, message);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            _serilogLogger.Debug(messageTemplate, propertyValues);
        }

        public void Info(string message, Exception exception)
        {
            _serilogLogger.Information(exception, message);
        }

        public void Info(string messageTemplate, params object[] propertyValues)
        {
            _serilogLogger.Information(messageTemplate, propertyValues);
        }

        public void Warn(string message, Exception exception)
        {
            _serilogLogger.Warning(exception, message);
        }

        public void Warn(string messageTemplate, params object[] propertyValues)
        {
            _serilogLogger.Warning(messageTemplate, propertyValues);
        }

        public void Error(string message, Exception exception)
        {
            _serilogLogger.Error(exception, message);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            _serilogLogger.Error(messageTemplate, propertyValues);
        }
    }

}
