using Abp.Dependency;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Tests
{
    public class TestConsoleLogger : ConsoleLogger, ILogger, ISingletonDependency
    {
        private string _lastMessage;

        public string GetLastMessage() => _lastMessage;
        protected override void Log(LoggerLevel loggerLevel, string loggerName, string message, Exception exception)
        {
            _lastMessage = message;
            base.Log(loggerLevel, loggerName, message, exception);
        }
    }
}
