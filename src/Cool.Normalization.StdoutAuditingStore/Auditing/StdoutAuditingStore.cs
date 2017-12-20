using Abp.Auditing;
using Castle.Core.Logging;
using Cool.Normalization.Utilities;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization
{
    public class StdoutAuditingStore : IAuditingStore
    {
        public const string NULL = nameof(NULL);

        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly ILogger _logger;
        private readonly IStdoutAuditStoreConfiguration _configuration;

        public StdoutAuditingStore(
            IRequestIdAccessor requestIdAccessor,
            IStdoutAuditStoreConfiguration configuration,
            ILogger logger)
        {
            this._requestIdAccessor = requestIdAccessor;
            this._logger = logger;
            this._configuration = configuration;
        }

        public Task SaveAsync(AuditInfo auditInfo)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
               .Append(_requestIdAccessor.RequestId ?? NULL)
               .Append(_configuration.LogSeparator)
               .Append(auditInfo.ExecutionTime.ToString("yyyy-MM-dd-HH:mm:ssz"))
               .Append(_configuration.LogSeparator)
               .Append(auditInfo.ServiceName)
               .Append(_configuration.LogSeparator)
               .Append(auditInfo.MethodName)
               .Append(_configuration.LogSeparator)
               .Append(auditInfo.ExecutionDuration)
               .Append(_configuration.LogSeparator)
               .Append(auditInfo?.Parameters
                ?.Replace("\r\n", _configuration.LineReplacement)
                ?.Replace("\r", _configuration.LineReplacement)
                ?.Replace("\n", _configuration.LineReplacement)
                ?? NULL)
               .Append(_configuration.LogSeparator)
               .Append(auditInfo.UserId?.ToString() ?? NULL)
               .Append(_configuration.LogSeparator)
               .Append(auditInfo.ClientIpAddress ?? NULL)
               .Append(_configuration.LogSeparator)
               .Append(auditInfo.Exception?.Message ?? NULL)
               .Append(_configuration.LogSeparator);

            if (auditInfo.Exception != null)
            {
                var stackTrace = new StackTrace(auditInfo.Exception);
                var stackFrame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
                if (stackFrame.HasMethod())
                {
                    var method = stackFrame.GetMethod()
                        .GetRealMethodFromAsyncMethodOrSelf();

                    stringBuilder
                    .Append(method?.DeclaringType?.FullName ?? NULL)
                    .Append(_configuration.LogSeparator)
                    .Append(method?.Name ?? NULL)
                    .Append(_configuration.LogSeparator);
                    if (method?.IsGenericMethod == true)
                    {
                        stringBuilder.Append(string.Join(",",
                            method
                            .GetGenericArguments()
                            .Select(t => t.FullName)));
                    }
                    else
                    {
                        stringBuilder.Append(NULL);
                    }

                }
                else
                {
                    stringBuilder.Append(string.Join(_configuration.LogSeparator, Enumerable.Repeat(NULL, 3)));
                }
                stringBuilder
                    .Append(_configuration.LogSeparator)
                    .Append(stackFrame.GetFileName() ?? NULL)
                    .Append(_configuration.LogSeparator)
                    .Append(stackFrame.GetFileLineNumber())
                    .Append(",")
                    .Append(stackFrame.GetFileColumnNumber());
            }
            else
            {
                stringBuilder.Append(string.Join(_configuration.LogSeparator, Enumerable.Repeat(NULL, 5)));
            }
            _logger.Info(stringBuilder.ToString());
            return Task.CompletedTask;
        }
    }
}