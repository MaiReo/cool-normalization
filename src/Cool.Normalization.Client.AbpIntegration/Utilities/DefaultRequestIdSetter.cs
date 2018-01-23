using System.Collections.Generic;
using Cool.Normalization;
using Cool.Normalization.Client;
using Cool.Normalization.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public class DefaultRequestIdSetter : IRequestIdSetter
#if NET452
        , Abp.Dependency.ITransientDependency
#endif
    {
        private readonly INormalizationConfiguration _configuration;

        public DefaultRequestIdSetter(INormalizationConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IRequestIdGenerator RequestIdGenerator { get; set; }

        public bool SetRequestId(Dictionary<string, string> headers)
        {
            var requestId = RequestIdGenerator?.RequestId;
            if (string.IsNullOrWhiteSpace( requestId ))
            {
                return false;
            }
            if (headers.ContainsKey( _configuration.RequestIdHeaderName ))
            {
                headers[_configuration.RequestIdHeaderName] = requestId;
            }
            else
            {
                headers.Add( _configuration.RequestIdHeaderName, requestId );
            }
            return true;
        }
    }
}
