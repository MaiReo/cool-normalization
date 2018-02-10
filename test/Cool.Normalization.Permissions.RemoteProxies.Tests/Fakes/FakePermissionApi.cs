using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using cool.permission.client.Model;
using cool.permission.client.Client;
using Cool.Normalization.Client;

namespace cool.permission.client.Api
{
    internal class FakePermissionApi : IApiAccessor<Configuration>, IPermissionApi
    {
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath() => "";

        public FakePermissionApi()
        {
            IsGrantWithHttpInfo_Delegate = (input) => new NormalizationResponse<IsGrantOutput>()
            {
                __normalization = true,
                Code = "00000000",
                Data = new IsGrantOutput()
            };
            RegisterWithHttpInfo_Delegate = (input) => new NormalizationResponse<object>()
            {
                __normalization = true,
                Code = "00000000",
            };
        }

        public Configuration Configuration { get; set; }
        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public ExceptionFactory ExceptionFactory { get; set; }

        /// <summary>
        /// Provides a RequestId Setter.
        /// </summary>
        public IRequestIdSetter RequestIdSetter { get; set; }

        #region Overload

        public NormalizationResponse<IsGrantOutput> IsGrant(IsGrantInput input = null)
            => IsGrantWithHttpInfo(input).Data;
        
        public async Task<NormalizationResponse<IsGrantOutput>> IsGrantAsync(IsGrantInput input = null)
            => (await IsGrantAsyncWithHttpInfo(input)).Data;
        
        public NormalizationResponse<object> Register(RegisterInput input = null)
            => RegisterWithHttpInfo(input).Data;
        
        public async Task<NormalizationResponse<object>> RegisterAsync(RegisterInput input = null)
            => (await RegisterAsyncWithHttpInfo(input)).Data;

        public Task<ApiResponse<NormalizationResponse<object>>> RegisterAsyncWithHttpInfo(RegisterInput input = null)
            => Task.FromResult(RegisterWithHttpInfo(input));

        public Task<ApiResponse<NormalizationResponse<IsGrantOutput>>> IsGrantAsyncWithHttpInfo(IsGrantInput input = null)
            => Task.FromResult(IsGrantWithHttpInfo(input));

        #endregion Overload

        public ApiResponse<NormalizationResponse<IsGrantOutput>> IsGrantWithHttpInfo(IsGrantInput input = null)
            => new ApiResponse<NormalizationResponse<IsGrantOutput>>(200, new Dictionary<string, string>(), IsGrantWithHttpInfo_Delegate.Invoke(input));

        public ApiResponse<NormalizationResponse<object>> RegisterWithHttpInfo(RegisterInput input = null)
            => new ApiResponse<NormalizationResponse<object>>(200, new Dictionary<string, string>(), RegisterWithHttpInfo_Delegate.Invoke(input));

        public Func<IsGrantInput, NormalizationResponse<IsGrantOutput>>  IsGrantWithHttpInfo_Delegate { get; set; }

        public Func<RegisterInput, NormalizationResponse<object>> RegisterWithHttpInfo_Delegate { get; set; }

        

        
        
    }
}