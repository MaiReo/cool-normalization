using Cool.Normalization.Permissions;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PermissionServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services,
            string authority = CoolAuthenticationDefaults.Authority,
            string apiName = CoolAuthenticationDefaults.ApiName,
            string apiSecret = CoolAuthenticationDefaults.ApiSecret)
        {
            services.AddAuthentication(
                IdentityServerAuthenticationDefaults.AuthenticationScheme )
            .AddIdentityServerAuthentication( options =>
            {
                options.Authority = authority ?? CoolAuthenticationDefaults.Authority
                    ?? throw new ArgumentNullException( nameof( authority ) );

                options.RequireHttpsMetadata = CoolAuthenticationDefaults.RequireHttpsMetadata;

                options.ApiName = apiName ?? CoolAuthenticationDefaults.ApiName
                    ?? throw new ArgumentNullException( nameof( apiName ) );

                options.ApiSecret = apiSecret ?? CoolAuthenticationDefaults.ApiSecret
                    ?? throw new ArgumentNullException( nameof( apiSecret ) );
                options.Validate();
            } );

            return services;
        }


        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            return app.UseAuthentication();
        }

    }
}
