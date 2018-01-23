using Abp.Dependency;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cool.Normalization.Utilities
{
    public class ErrorMessageGenerator : IErrorMessageGenerator, ISingletonDependency
    {

        public const string DEFAULT_ERROR_MESSAGE = "ERROR";
        public string GetErrorMessage( Exception exception )
        {
            if (exception == null)
            {
                return string.Empty;
            }

            if (exception is AbpValidationException abpValiEx)
            {
                var firstError = abpValiEx.ValidationErrors?.FirstOrDefault();

                return firstError?.ErrorMessage ?? abpValiEx.Message;
            }

            if (exception is Abp.Authorization.AbpAuthorizationException abpAuEx)
            {
                return abpAuEx.Message;
            }

            if (exception is Abp.AbpException abpEx)
            {
                return abpEx.Message;
            }

            return exception.Message;
        }
    }
}
