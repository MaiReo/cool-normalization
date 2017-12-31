#if NETSTANDARD2_0
using RestSharp.Portable;
#elif NET45
using RestSharp;
#endif
using System;

namespace Cool.Normalization.Client
{
    /// <summary>
    /// A delegate to ExceptionFactory method
    /// </summary>
    /// <param name="methodName">Method name</param>
    /// <param name="response">Response</param>
    /// <returns>Exceptions</returns>
    public delegate Exception ExceptionFactory( string methodName, IRestResponse response );
}
