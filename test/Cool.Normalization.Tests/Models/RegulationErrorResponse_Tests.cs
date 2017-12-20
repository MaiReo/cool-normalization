using Cool.Normalization.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class normalizationErrorResponse_Tests
    {
        [Fact]
        public void Property_Tests()
        {
            var requestId = "req";
            var code = "cod";
            var message = "msg";
            var errorResponse = new NormalizationErrorResponse(requestId,code,message);

            errorResponse.RequestId.ShouldBe( requestId );
            errorResponse.Code.ShouldBe( code );
            errorResponse.Message.ShouldBe( message );
        }
    }
}
