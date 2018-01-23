using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Events.Bus;
using Castle.Core.Logging;
using Cool.Normalization.Utilities;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization.Filters
{
    public class NormalizationAuthorizationFilter : IAsyncAuthorizationFilter, IFilterMetadata, ITransientDependency
    {
        private readonly IAuthorizationHelper _authorizationHelper;

        public IExceptionFilter ExceptionFilter { get; set; }

        public NormalizationAuthorizationFilter(
            IAuthorizationHelper authorizationHelper)
        {
            _authorizationHelper = authorizationHelper;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Allow Anonymous skips all authorization
            if (context.Filters.Any( item => item is IAllowAnonymousFilter ))
            {
                return;
            }

            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }
            var exception = default( Exception );
            var controllerDescriptor = context.ActionDescriptor.AsControllerActionDescriptor();
            try
            {
                await _authorizationHelper.AuthorizeAsync(
                            controllerDescriptor.MethodInfo,
                            controllerDescriptor.ControllerTypeInfo
                        );
            }
            catch (AbpAuthorizationException aae)
            {
                exception = new NormalizationException( 
                    levelCode: Codes.Level.AuthorizationFailed, 
                    message: aae.Message );
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (exception != null)
            {
                var exceptionContext = new ExceptionContext( context, context.Filters )
                {
                    Exception = exception,
                    Result = context.Result
                };
                ExceptionFilter?.OnException( exceptionContext );
                if ((!exceptionContext.ExceptionHandled)
                    && exceptionContext.Exception != null)
                {
                    throw exceptionContext.Exception;
                }
                context.Result = exceptionContext.Result;
            }
        }
    }
}