using Cool.Normalization.Configuration;
using Cool.Normalization.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Client
{
    public class PassthroughRequestIdSetter : IRequestIdSetter
#if NET452
        , Abp.Dependency.ITransientDependency
#endif
    {
        public INormalizationConfiguration Configuration { get; set; }
        public IRequestIdAccessor RequestIdAccessor { get; set; }
        public bool SetRequestId(Dictionary<string, string> headers)
        {
            var requestId = RequestIdAccessor?.RequestId;
            if (string.IsNullOrWhiteSpace( requestId ))
            {
                return false;
            }
            if (headers.ContainsKey( Configuration.RequestIdHeaderName ))
            {
                headers[Configuration.RequestIdHeaderName] = requestId;
            }
            else
            {
                headers.Add( Configuration.RequestIdHeaderName, requestId );
            }
            return true;
        }
    }
}
