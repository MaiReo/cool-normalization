using System;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Cool.Normalization.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;
using Microsoft.Extensions.Options;
using Abp.Dependency;
using Cool.Normalization.Utilities;
using Cool.Normalization.Auditing;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Cool.Normalization.Wrapping
{
    public class NormalizationObjectActionResultWrapper : IAbpActionResultWrapper
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IResultAuditingHelper _auditingResultHelper;
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IResultCodeGenerator _resultCodeGenerator;

        public NormalizationObjectActionResultWrapper( IResultAuditingHelper auditingResultHelper,
            IRequestIdAccessor requestIdAccessor, IResultCodeGenerator resultCodeGenerator,
            IServiceProvider serviceProvider )
        {
            this._auditingResultHelper = auditingResultHelper;
            this._requestIdAccessor = requestIdAccessor;
            this._resultCodeGenerator = resultCodeGenerator;
            this._serviceProvider = serviceProvider;
        }



        public void Wrap( ResultExecutingContext actionResult )
        {

            var objectResult = actionResult.Result as ObjectResult;
            if (objectResult == null)
            {
                throw new ArgumentException( $"{nameof( actionResult )} should be ObjectResult!" );
            }

            if ((objectResult.Value is NormalizationResponseBase))
            {
                return;
            }

            var requestId = _requestIdAccessor.GetRequestId( actionResult );
            var resultCode = _resultCodeGenerator.GetCode( actionResult.ActionDescriptor, actionResult.Controller?.GetType() );

            var normalizationResponse = new NormalizationResponse( requestId, resultCode, objectResult.Value );
            objectResult.Value = normalizationResponse;

            if (!objectResult.Formatters.Any( f => f is JsonOutputFormatter ))
            {
                objectResult.Formatters.Add(
                    new JsonOutputFormatter(
                        _serviceProvider.GetRequiredService<IOptions<MvcJsonOptions>>().Value.SerializerSettings,
                        _serviceProvider.GetRequiredService<ArrayPool<char>>()
                    )
                );
            }
            if (_auditingResultHelper.ShouldSaveAudit( (actionResult.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo ))
            {
                _auditingResultHelper.Save( normalizationResponse );
            }
        }
    }
}