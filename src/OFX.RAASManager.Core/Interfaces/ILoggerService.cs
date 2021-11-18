using System;

namespace OFX.RAASManager.Core.Interfaces
{
    public interface ILoggerService
    {
        void Debug(string message, Exception exception);

        void Debug(string messageTemplate, params object[] propertyValues);

        void Info(string message, Exception exception);

        void Info(string messageTemplate, params object[] propertyValues);

        void Warn(string message, Exception exception);

        void Warn(string messageTemplate, params object[] propertyValues);

        void Error(string message, Exception exception);

        void Error(string messageTemplate, params object[] propertyValues);
    }
}
