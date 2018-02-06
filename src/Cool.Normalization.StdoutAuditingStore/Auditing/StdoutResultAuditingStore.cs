#region Version=1.1.4
/**
* 
* StdoutResultAuditingStore
* 类。
* 实现IResultAuditingStore接口。
* 格式化输出审计日志交给ILogger以Info级别输出。
* 
*
* _serializer
* 只读字段。一个指向类型为JsonSerializer的实例的引用
*
* _stdoutAuditStoreconfiguration
* 只读字段。一个指向实现类型为IStdoutAuditStoreConfiguration的接口的类的实例的引用
*
* Logger
* 属性。一个指向实现类型为ILogger的接口的类的实例的引用
*
* StdoutResultAuditingStore()
* 构造函数。初始化成员_serializer，成员Logger的值。
*
* 可能引发的Code值
*
* 可能引发的异常
* 
*
* StdoutResultAuditingStore(IStdoutAuditStoreConfiguration)
* 构造函数。初始化成员_stdoutAuditStoreconfiguration的值。
*
* 可能引发的Code值
*
* 可能引发的异常
*
*
* Save(NormalizationResponseBase)
* 拼接字符串作为参数调用成员Logger的Info方法。
* 该字符串是一个
*   以"AUDIT-OUT"开头,
*
*   以调用成员"_stdoutAuditStoreconfiguration"的成员“LogSeparator”的get访问器的返回结果的值分隔的，   
*   追加调用类型为NormalizationResponseBase的实例的成员"RequestId”的get访问器的返回结果的值，   
*   追加调用类型为NormalizationResponseBase的实例的成员"Code”的get访问器的返回结果的值，   
*   通过调用成员_serializer的Serialize方法追加类型为NormalizationResponseBase的实例序列化为json格式的文本，

*   “不”包含换行符，
*   的字符串。
*
* 可能引发的Code值
*
* 可能引发的异常
*
*
* Task SaveAsync(NormalizationResponseBase)
* Save方法的异步版本
*
* 可能引发的Code值
*
* 可能引发的异常
*   TaskCanceledException
*   OperationCanceledException
*   AggregateException
*
*/
#endregion

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Cool.Normalization.Auditing;
using Cool.Normalization.Configuration;
using Cool.Normalization.Models;
using Cool.Normalization.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cool.Normalization
{
    public class StdoutResultAuditingStore : IResultAuditingStore
    {
        private readonly IStdoutAuditStoreConfiguration
            _stdoutAuditStoreconfiguration;

        private readonly JsonSerializer _serializer;

        public ILogger Logger { get; set; }

        public StdoutResultAuditingStore()
        {
            this._serializer = JsonSerializer.Create(
               new JsonSerializerSettings
               {
                   Formatting = Formatting.None,
                   ContractResolver
                    = new CamelCasePropertyNamesContractResolver(),
               } );
            Logger = NullLogger.Instance;
        }

        public StdoutResultAuditingStore(
            IStdoutAuditStoreConfiguration stdoutAuditStoreconfiguration)
            : this()
        {
            this._stdoutAuditStoreconfiguration = stdoutAuditStoreconfiguration;
        }
        /// <summary>
        /// 见 <see cref=“IResultAuditingStore.Save(NormalizationResponseBase)”/>
        /// </summary>
        public void Save(NormalizationResponseBase normalizationResponse)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
                .Append( "AUDIT-OUT" )
                .Append( _stdoutAuditStoreconfiguration.LogSeparator )
                .Append( normalizationResponse.RequestId ?? "NULL" )
                .Append( _stdoutAuditStoreconfiguration.LogSeparator )
                .Append( normalizationResponse.Code ?? "NULL" )
                .Append( _stdoutAuditStoreconfiguration.LogSeparator );
            using (var stringWriter = new StringWriter( stringBuilder ))
            {
                _serializer.Serialize( stringWriter, normalizationResponse );
            }
            stringBuilder
                .Replace( "\r\n", _stdoutAuditStoreconfiguration.LineReplacement )
                .Replace( "\r", _stdoutAuditStoreconfiguration.LineReplacement )
                .Replace( "\n", _stdoutAuditStoreconfiguration.LineReplacement );

            Logger.Info( stringBuilder.ToString() );

            stringBuilder.Clear();
        }
        /// <summary>
        /// A Task-based Calling of <see cref=“Save(NormalizationResponseBase)”/>
        /// </summary>
        /// <exception cref="TaskCanceledException"/>
        /// <exception cref="System.OperationCanceledException"/>
        /// <exception cref="System.AggregateException"/>
        public async Task SaveAsync(
            NormalizationResponseBase normalizationResponse)
            => await Task.Run( () => Save( normalizationResponse ) );
    }
}