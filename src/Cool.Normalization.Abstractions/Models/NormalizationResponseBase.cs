using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Models
{
    public abstract class NormalizationResponseBase
    {
        public NormalizationResponseBase()
        {
            __normalization = true;
        }

        public NormalizationResponseBase( string requestId, string code ) : this()
        {
            this.RequestId = requestId;
            this.Code = code;
        }
        public NormalizationResponseBase( string requestId, string code, string message ) : this( requestId, code )
        {
            this.Message = message;
        }

        public string RequestId { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public bool __normalization { get; set; }
    }
}
