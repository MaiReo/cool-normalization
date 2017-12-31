using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Client
{
    public class NormalizationResponse
    {
        public virtual string RequestId { get; set; }

        public virtual string Code { get; set; }

        public virtual string Message { get; set; }

        public virtual bool __normalization { get; set; }
    }

    public class NormalizationResponse<T> : NormalizationResponse
    {
        public T Data { get; set; }
    }
}
