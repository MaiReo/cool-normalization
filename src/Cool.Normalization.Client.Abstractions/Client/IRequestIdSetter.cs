using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Client
{
    public interface IRequestIdSetter
    {
        bool SetRequestId( Dictionary<String, String> headers );
    }
}
