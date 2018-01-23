﻿using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerAppBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerEtc(this IApplicationBuilder app, string uniqueName)
        {
            if (string.IsNullOrWhiteSpace( uniqueName ))
                throw new ArgumentNullException( nameof( uniqueName ) );
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger( options =>
            {
                options.PreSerializeFilters.Add( (s, req) => s.Host = req.Host.Value );
                options.PreSerializeFilters.Add( (s, req) => s.Schemes = new List<string> { req.Scheme } );
            } );
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI( options =>
            {
                options.SwaggerEndpoint( "/swagger/v1/swagger.json", uniqueName + " API V1" );
            } ); // URL: /swagger
            return app;
        }
    }
}
