using Cool.Normalization.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class normalizationResponse_Tests
    {
        [Fact]
        public void Property_Tests()
        {
            var requestId = "req";
            var code = "cod";
            var data = new object();
            var errorResponse = new NormalizationResponse(requestId,code, data );

            errorResponse.RequestId.ShouldBe( requestId );
            errorResponse.Code.ShouldBe( code );
            errorResponse.Data.ShouldBe( data );
        }
    }
}
