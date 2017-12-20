using Abp.Runtime.Validation;
using Cool.Normalization.Utilities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class ErrorMessageGenerator_Tests : MicroServicesnormalizationTestBase
    {

        private readonly IErrorMessageGenerator _errorMessageGenerator;

        public ErrorMessageGenerator_Tests()
        {
            _errorMessageGenerator = Resolve<IErrorMessageGenerator>();
        }
        [Fact]
        public void GetErrorMessage_Excepion_Test()
        {
            var exception = new Exception( "errmsg" );
            var errMsg = _errorMessageGenerator.GetErrorMessage( exception );

            errMsg.ShouldBe( exception.Message );
        }
        [Fact]
        public void GetErrorMessage_AbpValidationException_Test()
        {
            var errors = new List<ValidationResult> { new ValidationResult( "45|validationError", new[] { "member1" } ) };

            var exception = new AbpValidationException( "errormsg", errors );

            var errMsg = _errorMessageGenerator.GetErrorMessage( exception );

            errMsg.ShouldBe( "45|validationError" );
        }
    }
}
