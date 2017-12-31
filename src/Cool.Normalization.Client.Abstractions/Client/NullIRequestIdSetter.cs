using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Client
{
    public class NullRequestIdSetter : IRequestIdSetter
    {
        public static NullRequestIdSetter Instance => new NullRequestIdSetter();
        public bool SetRequestId( Dictionary<String, String> headers ) => false;
    }
}
