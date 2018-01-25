#region 程序集 Version=1.0.6
/*
 * IResultAuditingStore的默认实现类
 * 将包装后的信息，序列化为json通过Logger直接输出。
 *  
 * 使用场景
 *  整个封装的框架中，包含一个StdoutAuditingStore项目，该项目负责实现了具体的日志输出行为。此默认类，一般情况下不会构造对象并使用。
 */
#endregion

using System.Threading.Tasks;
using Cool.Normalization.Models;
using Castle.Core.Logging;
using Abp.Dependency;

namespace Cool.Normalization.Auditing
{
    public class SimpleLogResultAuditingStore : IResultAuditingStore
    {

        public SimpleLogResultAuditingStore()
        {
            Logger = NullLogger.Instance;
            Instance = this;
        }

        public ILogger Logger { get; set; }

        public static SimpleLogResultAuditingStore Instance { get; private set; }

        public void Save( NormalizationResponseBase normalizationResponse )
        {
            if (normalizationResponse == null)
            {
                return;
            }
            Logger.Info( Newtonsoft.Json.JsonConvert.SerializeObject( normalizationResponse ) );
        }

        public Task SaveAsync( NormalizationResponseBase normalizationResponse )
        {
            Save( normalizationResponse );
            return Task.CompletedTask;
        }
    }
}