using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class NormalizationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNormalization(this IApplicationBuilder app,
            bool? useAuth = default,
            bool? useSwagger = default)
        {
            if (useAuth == true)
            {
                app.UseAuth();
            }
            if (useSwagger != false)
            {
                app.UseSwaggerEtc( NormalizationServiceCollectionExtensions.DisplayName );
            }
            return app;
        }
    }
}
