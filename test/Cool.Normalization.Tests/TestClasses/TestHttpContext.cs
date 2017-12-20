using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;
using System.Threading;

namespace Cool.Normalization.Tests
{
    public class TestHttpContext : HttpContext
    {

        public TestHttpContext( HttpRequest request, HttpResponse response )
        {
            this._httpRequest = request;
            this._httpResponse = response;
        }

        public override IFeatureCollection Features => null;

        private HttpRequest _httpRequest;
        public override HttpRequest Request => _httpRequest;

        public void SetHttpRequest( HttpRequest httpRequest )
        {
            _httpRequest = httpRequest;
        }

        private HttpResponse _httpResponse;
        public override HttpResponse Response => _httpResponse;

        public void SetHttpResponse( HttpResponse httpResponse )
        {
            _httpResponse = httpResponse;
        }

        public override ConnectionInfo Connection => null;

        public override WebSocketManager WebSockets => null;

        [Obsolete]
        public override AuthenticationManager Authentication => null;

        public override ClaimsPrincipal User { get; set; }
        public override IDictionary<object, object> Items { get; set; }
        public override IServiceProvider RequestServices { get; set; }
        public override CancellationToken RequestAborted { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ISession Session { get; set; }

        public override void Abort()
        {

        }
        static TestHttpContext()
        {
            var request = new TestHttpRequest();
            var response = new TestHttpResponse();
            Current = new TestHttpContext( request, response );
        }
        public static HttpContext Current { get; set; }
    }



}
