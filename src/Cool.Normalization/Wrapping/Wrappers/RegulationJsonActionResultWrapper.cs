using Abp.AspNetCore.Mvc.Results.Wrapping;
using Cool.Normalization.Models;
using Cool.Normalization.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Cool.Normalization.Auditing;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Cool.Normalization.Wrapping
{
    public class normalizationJsonActionResultWrapper : IAbpActionResultWrapper
    {
        private readonly IResultAuditingHelper _auditingResultHelper;
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IResultCodeGenerator _resultCodeGenerator;

        public normalizationJsonActionResultWrapper( Auditing.IResultAuditingHelper auditingResultHelper,
            IRequestIdAccessor requestIdAccessor, IResultCodeGenerator resultCodeGenerator )
        {
            this._auditingResultHelper = auditingResultHelper;
            this._requestIdAccessor = requestIdAccessor;
            this._resultCodeGenerator = resultCodeGenerator;
        }

        public void Wrap( ResultExecutingContext actionResult )
        {
            var jsonResult = actionResult.Result as JsonResult;
            if (jsonResult == null)
            {
                throw new ArgumentException( $"{nameof( actionResult )} should be JsonResult!" );
            }
            if (jsonResult.Value is NormalizationResponseBase) return;

            var requestId = _requestIdAccessor.GetRequestId( actionResult );
            var resultCode = _resultCodeGenerator.GetCode( actionResult.ActionDescriptor, actionResult.Controller?.GetType() );
            var normalizationResponse = new NormalizationResponse( requestId, resultCode, jsonResult.Value );

            jsonResult.Value = normalizationResponse;

            if (_auditingResultHelper.ShouldSaveAudit( (actionResult.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo ))
            {
                _auditingResultHelper.Save( normalizationResponse );
            }
        }
    }
}