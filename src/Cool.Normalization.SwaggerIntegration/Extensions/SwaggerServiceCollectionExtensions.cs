#region Version=1.0.0
/*
 * 将Swagger加入到AspNet管道的简化同时处理一些小问题
 *
 * 添加支持bearer令牌验证
 *
 */
#endregion Version
using Cool.Nomalization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerEtc(this IServiceCollection services,
            string uniqueName,
            bool hasAuth = false)
        {
            if (string.IsNullOrWhiteSpace( uniqueName ))
                throw new ArgumentNullException( nameof( uniqueName ) );
            services.AddSwaggerGen( options =>
            {
                options.SwaggerDoc( "v1", new Info { Title = uniqueName + " API", Version = "v1" } );
                options.DocInclusionPredicate( (docName, description) => true );
                if (hasAuth)
                {
                    options.AddSecurityDefinition( "bearerAuth", new ApiKeyScheme()
                    {
                        Description = "JWT Authorization header using the Bearer scheme." +
                        " Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    } );
                }
                options.OperationFilter<RemovePreFixOperationFilter>();
            } );
            return services;
        }
    }
}
