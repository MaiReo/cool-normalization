using Microsoft.AspNetCore.Http;

namespace Cool.Normalization.Tests
{
    internal class TestHttpContextAccessor : IHttpContextAccessor
    {

        public HttpContext HttpContext { get => TestHttpContext.Current; set => TestHttpContext.Current = value; }
    }
}