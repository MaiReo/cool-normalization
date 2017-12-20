using Abp.AspNetCore.Mvc.Results.Wrapping;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Abp;
using Microsoft.AspNetCore.Mvc;
using Abp.Dependency;
using Cool.Normalization.Utilities;
using Cool.Normalization.Auditing;

namespace Cool.Normalization.Wrapping
{
    public class NormalizationActionResultWrapperFactory : IAbpActionResultWrapperFactory, ITransientDependency
    {
        private readonly IResultAuditingHelper _auditingResultHelper;

        public IIocResolver IocResolver { get; set; }

        public NormalizationActionResultWrapperFactory( IResultAuditingHelper auditingResultHelper)
        {
            this._auditingResultHelper = auditingResultHelper;
        }

        public IAbpActionResultWrapper CreateFor( ResultExecutingContext actionResult )
        {
            Check.NotNull( actionResult, nameof( actionResult ) );

            var requestIdAccessor = IocResolver.Resolve<IRequestIdAccessor>();
            var resultCodeGenerator = IocResolver.Resolve<IResultCodeGenerator>();

            if (actionResult.Result is ObjectResult)
            {
                return new normalizationObjectActionResultWrapper( _auditingResultHelper, requestIdAccessor,
                    resultCodeGenerator, actionResult.HttpContext.RequestServices );
            }

            if (actionResult.Result is JsonResult)
            {
                return new normalizationJsonActionResultWrapper( _auditingResultHelper, requestIdAccessor, resultCodeGenerator );
            }

            if (actionResult.Result is EmptyResult)
            {
                return new normalizationEmptyActionResultWrapper( _auditingResultHelper, requestIdAccessor, resultCodeGenerator );
            }
            return new NullnormalizationActionResultWrapper();
        }
    }
}
