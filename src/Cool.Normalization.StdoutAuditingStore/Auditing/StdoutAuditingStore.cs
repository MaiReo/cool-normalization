#region Version=1.1.4
/**
* 
* StdoutAuditingStore
* 类。
* 实现IAuditingStore接口。
* 格式化输入审计日志交给ILogger以Info级别输出。
* 
* NULL
* 常量字段。值永远是“NULL”
*
* _requestIdAccessor
* 只读字段。一个指向实现类型为IRequestIdAccessor的接口的类的实例的引用
*
* _logger
* 只读字段。一个指向实现类型为ILogger的接口的类的实例的引用
*
* _configuration
* 只读字段。一个指向实现类型为IStdoutAuditStoreConfiguration的接口的类的实例的引用
*
* StdoutAuditingStore(IRequestIdAccessor, IStdoutAuditStoreConfiguration, ILogger)
* 构造函数。初始化成员_requestIdAccessor,成员_configuration,成员_logger的值。

* 可能引发的Code值
*
* 可能引发的异常
* 
* Task SaveAsync(AuditInfo)
* 拼接字符串作为参数调用成员_logger的Info方法。
* 该字符串是一个：
*   以“AUDIT-IN”开头，
*
*   以调用成员"_configuration"的成员“LogSeparator”的get访问器的返回结果的值分隔的，
*   追加调用成员"_requestIdAccessor"的成员“RequestId”的get访问器的返回结果的值，
*   
*   追加类型为AuditInfo的对象实例的公有成员的值，
*   对类型为AuditInfo的实例对象的所属成员Parameters调用get访问器的值并将换行符替换为
*   调用成员"_configuration"的成员“LineReplacement”的get访问器的返回结果的值，
*
*   对类型为AuditInfo的实例对象的所属成员Exception调用get访问器的值并将换行符替换为
*   调用成员"_configuration"的成员“LineReplacement”的get访问器的返回结果的值，
*
*   如果存在异常信息，不对类型为NormalizationException的异常进行堆栈信息跟踪。
*
*   如果存在异常信息，进行堆栈信息跟踪。
*   找出最上层的堆栈信息。
*   追加引发异常的方法的所在类的完整名称，
*   追加引发异常的方法的名称，
*   追加引发异常的方法的所有的泛型参数类型的以逗号分隔的完整类型名称，
*
*   追加引发异常的方法的所在类的所在文件的编译时的完整路径,
*   追加以逗号分隔的引发异常的代码的所在行号和列号，
*
*   “不”包含换行符，
*   的字符串。
*
*   字符串格式
*   AUDIT-IN RequestId	ExecTime	ServiceName	MethodName	Duration	Input	UserId	IPAddress	ExceptionType	ExceptionMessage	MethodDeclaringTypeFullName	MethodName	MethodGenericArgumentTypes	FileName	LineColumn
*   成功示例
*   AUDIT-IN^f8f0b7f804a411e89391b31d96f27d11^2018-01-29 11:32:04+8^ServiceName^MethodName^300^{"input":{"argument1:value1"}}}^2^[::1]^NULL^NULL^NULL^NULL^NULL^NULL^NULL;
*   异常示例1
*   AUDIT-IN^f8f0b7f804a411e89391b31d96f27d11^2018-01-29 11:32:04+8^ServiceName^MethodName^300^{"input":{"argument1:value1"}}}^2^[::1]^NormalizationException^ExceptionMessage^NULL^NULL^NULL^NULL^NULL;
*   异常示例2
*   AUDIT-IN^f8f0b7f804a411e89391b31d96f27d11^2018-01-29 11:32:04+8^ServiceName^MethodName^300^{"input":{"argument1:value1"}}}^2^[::1]^Exception^ExceptionMessage^Your.Namespace.SomeClass^ThatMethod^GenericArgumentTypeFullName^/Path/To/File/SomeClass.cs^Line,Column;
*
* 可能引发的Code值
*
* 可能引发的异常
*   
*/
#endregion Version
using Abp.Auditing;
using Castle.Core.Logging;
using Cool.Normalization.Configuration;
using Cool.Normalization.Utilities;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.Auditing
{
    ///<summary>
    ///格式化输入审计日志交给ILogger以Info级别输出。
    ///</summary>
    public class StdoutAuditingStore : IAuditingStore
    {
        public const string NULL = nameof( NULL );

        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly ILogger _logger;
        private readonly IStdoutAuditStoreConfiguration _configuration;

        public StdoutAuditingStore(
            IRequestIdAccessor requestIdAccessor,
            IStdoutAuditStoreConfiguration configuration,
            ILogger logger )
        {
            this._requestIdAccessor = requestIdAccessor;
            this._logger = logger;
            this._configuration = configuration;
        }

        public Task SaveAsync( AuditInfo auditInfo )
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
               .Append( "AUDIT-IN" )
               .Append( _configuration.LogSeparator )
               .Append( _requestIdAccessor.RequestId ?? NULL )
               .Append( _configuration.LogSeparator )
               .Append( auditInfo.ExecutionTime.ToString( "yyyy-MM-dd-HH:mm:ssz" ) )
               .Append( _configuration.LogSeparator )
               .Append( auditInfo.ServiceName )
               .Append( _configuration.LogSeparator )
               .Append( auditInfo.MethodName )
               .Append( _configuration.LogSeparator )
               .Append( auditInfo.ExecutionDuration )
               .Append( _configuration.LogSeparator )
               .Append( auditInfo?.Parameters
                ?.Replace( "\r\n", _configuration.LineReplacement )
                ?.Replace( "\r", _configuration.LineReplacement )
                ?.Replace( "\n", _configuration.LineReplacement )
                ?? NULL )
               .Append( _configuration.LogSeparator )
               .Append( auditInfo.UserId?.ToString() ?? NULL )
               .Append( _configuration.LogSeparator )
               .Append( auditInfo.ClientIpAddress ?? NULL )
               .Append( _configuration.LogSeparator );

            if (auditInfo.Exception == null)
            {
                stringBuilder.Append( string.Join( _configuration.LogSeparator, Enumerable.Repeat( NULL, 7 ) ) );
            }
            else
            {
                var exceptionType = auditInfo.Exception.GetType();
                stringBuilder
                 .Append( exceptionType.Name )
                 .Append( _configuration.LogSeparator )
                 .Append( auditInfo.Exception.Message
                    ?.Replace( "\r\n", _configuration.LineReplacement )
                    ?.Replace( "\r", _configuration.LineReplacement )
                    ?.Replace( "\n", _configuration.LineReplacement ) ?? NULL )
                 .Append( _configuration.LogSeparator );

                //skip stack tracing
                if (auditInfo.Exception is NormalizationException)
                {
                    stringBuilder.Append( string.Join( _configuration.LogSeparator, Enumerable.Repeat( NULL, 3 ) ) );
                }
                else
                {
                    var stackTrace = new StackTrace( auditInfo.Exception );
                    var stackFrame = stackTrace.GetFrame( stackTrace.FrameCount - 1 );
                    if (stackFrame.HasMethod())
                    {
                        var method = stackFrame.GetMethod()
                            .GetRealMethodFromAsyncMethodOrSelf();

                        stringBuilder
                        .Append( method?.DeclaringType?.FullName ?? NULL )
                        .Append( _configuration.LogSeparator )
                        .Append( method?.Name ?? NULL )
                        .Append( _configuration.LogSeparator );
                        if (method?.IsGenericMethod == true)
                        {
                            stringBuilder.Append( string.Join( ",",
                                method
                                .GetGenericArguments()
                                .Select( t => t.FullName ) ) );
                        }
                        else
                        {
                            stringBuilder.Append( NULL );
                        }
                    }
                    else
                    {
                        stringBuilder.Append( string.Join( _configuration.LogSeparator, Enumerable.Repeat( NULL, 3 ) ) );
                    }
                    stringBuilder
                    .Append( _configuration.LogSeparator )
                    .Append( stackFrame.GetFileName() ?? NULL )
                    .Append( _configuration.LogSeparator )
                    .Append( stackFrame.GetFileLineNumber() )
                    .Append( "," )
                    .Append( stackFrame.GetFileColumnNumber() );

                }
            }

            _logger.Info( stringBuilder.ToString() );
            return Task.CompletedTask;
        }
    }
}