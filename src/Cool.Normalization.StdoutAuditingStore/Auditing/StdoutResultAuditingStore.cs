using System.IO;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Cool.Normalization.Auditing;
using Cool.Normalization.Models;
using Cool.Normalization.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cool.Normalization
{
    public class StdoutResultAuditingStore : IResultAuditingStore
    {
        private readonly ILogger _logger;
        private readonly IStdoutAuditStoreConfiguration _configuration;
        private readonly JsonSerializer _serializer;

        public StdoutResultAuditingStore(
            IStdoutAuditStoreConfiguration configuration,
            ILogger logger)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._serializer = JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    ContractResolver
                = new CamelCasePropertyNamesContractResolver(),
                });
        }

        public void Save(NormalizationResponseBase normalizationResponse)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
                .Append("AUDIT-OUT")
                .Append( _configuration.LogSeparator )
                .Append(normalizationResponse.RequestId ?? "NULL")
                .Append(_configuration.LogSeparator)
                .Append(normalizationResponse.Code ?? "NULL")
                .Append(_configuration.LogSeparator);
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                _serializer.Serialize(stringWriter, normalizationResponse);
            }
            stringBuilder
                .Replace("\r\n", _configuration.LineReplacement)
                .Replace("\r", _configuration.LineReplacement)
                .Replace("\n", _configuration.LineReplacement);

            _logger.Info(stringBuilder.ToString());

            stringBuilder.Clear();
        }

        public async Task SaveAsync(
            NormalizationResponseBase normalizationResponse)
            => await Task.Run(() => Save(normalizationResponse));
    }
}