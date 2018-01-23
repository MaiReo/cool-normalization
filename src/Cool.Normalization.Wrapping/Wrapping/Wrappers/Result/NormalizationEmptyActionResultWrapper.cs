using Abp.AspNetCore.Mvc.Results.Wrapping;
using Cool.Normalization.Auditing;
using Cool.Normalization.Models;
using Cool.Normalization.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization.Wrapping
{
    public class NormalizationEmptyActionResultWrapper : IAbpActionResultWrapper
    {
        private readonly IResultAuditingHelper _auditingResultHelper;
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IResultCodeGenerator _resultCodeGenerator;

        public NormalizationEmptyActionResultWrapper( IResultAuditingHelper auditingResultHelper,
            IRequestIdAccessor requestIdAccessor,
            IResultCodeGenerator resultCodeGenerator )
        {
            this._auditingResultHelper = auditingResultHelper;
            this._requestIdAccessor = requestIdAccessor;
            this._resultCodeGenerator = resultCodeGenerator;
        }

        public void Wrap( ResultExecutingContext actionResult )
        {
            var resultId = _requestIdAccessor.GetRequestId( actionResult );
            var resultCode = _resultCodeGenerator.GetCode( actionResult.ActionDescriptor, actionResult.Controller?.GetType() );

            var normalizationResponse = new NormalizationResponse( resultId, resultCode );
            actionResult.Result = new ObjectResult( normalizationResponse );

            if (_auditingResultHelper.ShouldSaveAudit( (actionResult.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo ))
            {
                _auditingResultHelper.Save( normalizationResponse );
            }
        }
    }
}