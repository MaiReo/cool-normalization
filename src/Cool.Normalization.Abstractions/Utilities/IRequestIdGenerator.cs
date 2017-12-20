using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization
{
    public interface IRequestIdGenerator
    {
        string RequestId { get; }
    }
}
