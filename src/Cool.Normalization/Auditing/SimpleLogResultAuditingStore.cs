using System.Threading.Tasks;
using Cool.Normalization.Models;
using Castle.Core.Logging;
using Abp.Dependency;

namespace Cool.Normalization.Auditing
{
    public class SimpleLogResultAuditingStore : IResultAuditingStore, ISingletonDependency
    {

        public SimpleLogResultAuditingStore()
        {
            Logger = NullLogger.Instance;
            Instance = this;
        }

        public ILogger Logger { get; set; }

        public static SimpleLogResultAuditingStore Instance { get; internal set; }

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