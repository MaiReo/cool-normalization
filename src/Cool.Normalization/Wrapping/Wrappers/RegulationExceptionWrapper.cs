using Abp.AspNetCore.Mvc.Extensions;
using Abp.AspNetCore.Mvc.Results;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Events.Bus;
using Abp.Events.Bus.Exceptions;
using Abp.Runtime.Validation;
using Cool.Normalization.Auditing;
using Cool.Normalization.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Cool.Normalization.Wrapping
{
    public class normalizationExceptionWrapper : INormalizationExceptionWrapper
    {
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IResultCodeGenerator _resultCodeGenerator;
        private readonly IErrorMessageGenerator _errorMessageGenerator;
        private readonly IEventBus _eventBus;
        private readonly IResultAuditingHelper _resultAuditingHelper;
        public normalizationExceptionWrapper( IEventBus eventBus,
            IRequestIdAccessor requestIdAccessor,
            IResultCodeGenerator resultCodeGenerator,
            IErrorMessageGenerator errorMessageGenerator ,
           IResultAuditingHelper resultAuditingHelper )
        {
            this._eventBus = eventBus;
            this._requestIdAccessor = requestIdAccessor;
            this._resultCodeGenerator = resultCodeGenerator;
            this._errorMessageGenerator = errorMessageGenerator;
            this._resultAuditingHelper = resultAuditingHelper;
        }


        public void Wrap( ExceptionContext context )
        {
            if (!ActionResultHelper.IsObjectResult( context.ActionDescriptor.GetMethodInfo().ReturnType ))
            {
                return;
            }

            context.HttpContext.Response.StatusCode = GetStatusCode( context );
            var normalizationResponse = new Models.NormalizationErrorResponse(
                    _requestIdAccessor.GetRequestId( context ),
                    _resultCodeGenerator.GetCode( context,
                        context.ActionDescriptor.AsControllerActionDescriptor()?.ControllerTypeInfo ),
                    _errorMessageGenerator.GetErrorMessage( context.Exception )
            );
            context.Result = new ObjectResult( normalizationResponse );

            try
            {
                _eventBus.Trigger( this, new AbpHandledExceptionData( context.Exception ) );
            }
            finally
            {
                context.Exception = null; //Handled!

                context.ExceptionHandled = true;

                if (_resultAuditingHelper.ShouldSaveAudit( context.ActionDescriptor.AsControllerActionDescriptor()?.MethodInfo ))
                {
                    _resultAuditingHelper.Save( normalizationResponse );
                }
            }

            

        }
        private int GetStatusCode( ExceptionContext context )
        {
            if (context.Exception is AbpAuthorizationException)
            {
                return context.HttpContext.User.Identity.IsAuthenticated
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;
            }

            //All Success Response.
            return (int)HttpStatusCode.OK;

            //if (context.Exception is AbpValidationException)
            //{
            //    return (int)HttpStatusCode.BadRequest;
            //}

            //if (context.Exception is EntityNotFoundException)
            //{
            //    return (int)HttpStatusCode.NotFound;
            //}

            //return (int)HttpStatusCode.InternalServerError;
        }
    }
}
