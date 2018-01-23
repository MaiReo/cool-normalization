using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cool.Normalization.Tests
{
    internal class TestHttpResponse : HttpResponse
    {
        private HttpContext _httpContext;

        public TestHttpResponse()
        {
            Headers = new HeaderDictionary();
        }
        public TestHttpResponse( HttpContext httpContext ) : this()
        {
            _httpContext = httpContext;
        }

        public override HttpContext HttpContext => _httpContext;

        public void SetHttpContext( HttpContext httpContext )
        {
            this._httpContext = httpContext;
        }

        public override int StatusCode { get; set; }

        public override IHeaderDictionary Headers { get; }

        public override Stream Body { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }

        public override IResponseCookies Cookies => null;
        public override bool HasStarted => true;

        public override void OnCompleted( Func<object, Task> callback, object state )
        {

        }

        public override void OnStarting( Func<object, Task> callback, object state )
        {

        }

        public override void Redirect( string location, bool permanent )
        {

        }
    }
}