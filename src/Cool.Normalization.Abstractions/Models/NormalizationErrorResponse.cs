using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Models
{
    public class NormalizationErrorResponse : NormalizationResponseBase
    {
        public NormalizationErrorResponse( string requestId, string code, string message ) : base( requestId, code, message )
        {
        }
    }
}
