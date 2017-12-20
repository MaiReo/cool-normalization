using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization
{
    public class RequestIdGenerator : IRequestIdGenerator
    {
        public RequestIdGenerator()
        {
            RequestId = Guid.NewGuid().ToString( "N" ).ToLowerInvariant();
        }

        public string RequestId { get; }
    }
}
