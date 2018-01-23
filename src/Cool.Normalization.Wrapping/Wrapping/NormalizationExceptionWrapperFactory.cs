using Abp.Dependency;
using Abp.Events.Bus;
using Cool.Normalization.Auditing;
using Cool.Normalization.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Wrapping
{
    public class NormalizationExceptionWrapperFactory : INormalizationExceptionWrapperFactory, ITransientDependency
    {
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IResultCodeGenerator _resultCodeGenerator;
        private readonly IErrorMessageGenerator _errorMessageGenerator;
        private readonly IResultAuditingHelper _resultAuditingHelper;
        public NormalizationExceptionWrapperFactory( IRequestIdAccessor requestIdAccessor,
            IResultCodeGenerator resultCodeGenerator,
            IErrorMessageGenerator errorMessageGenerator,
            IResultAuditingHelper resultAuditingHelper)
        {
            this._requestIdAccessor = requestIdAccessor;
            this._resultCodeGenerator = resultCodeGenerator;
            this._errorMessageGenerator = errorMessageGenerator;
            this._resultAuditingHelper = resultAuditingHelper;

            EventBus = NullEventBus.Instance;
        }
        public IEventBus EventBus { get; set; }


        public INormalizationExceptionWrapper CreateFor( ExceptionContext exceptionContext )
        {
            return new NormalizationExceptionWrapper( EventBus, 
                _requestIdAccessor,
                _resultCodeGenerator,
                _errorMessageGenerator, 
                _resultAuditingHelper );
        }
    }
}
