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
        private readonly INormalizationConfiguration _configuration;
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
            INormalizationConfiguration configuration,
            IStdoutAuditStoreConfiguration stdoutAuditStoreconfiguration)
            : this()
        {
            this._configuration = configuration;
            this._stdoutAuditStoreconfiguration = stdoutAuditStoreconfiguration;
        }

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

        public async Task SaveAsync(
            NormalizationResponseBase normalizationResponse)
            => await Task.Run( () => Save( normalizationResponse ) );
    }
}