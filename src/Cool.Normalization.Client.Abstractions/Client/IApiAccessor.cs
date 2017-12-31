using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Client
{
    public interface IApiAccessor<T> : IApiAccessor
    {
        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        T Configuration { get; set; }
    }
    /// <summary>
    /// Represents configuration aspects required to interact with the API endpoints.
    /// </summary>
    public interface IApiAccessor
    {
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        string GetBasePath();

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        ExceptionFactory ExceptionFactory { get; set; }

        /// <summary>
        /// Provides a RequestId Setter.
        /// </summary>
        IRequestIdSetter RequestIdSetter { get; set; }
    }
}
