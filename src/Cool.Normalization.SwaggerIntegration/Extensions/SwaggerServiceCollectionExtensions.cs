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
using System.IO;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="friendlyName">友好名称</param>
        /// <param name="hasAuth">是否存在Bearer验证</param>
        /// <param name="xmlComments">附加的xml文档</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerEtc(this IServiceCollection services,
            string friendlyName,
            bool hasAuth = false,
            IEnumerable<string> xmlComments = default)
        {
            if (string.IsNullOrWhiteSpace( friendlyName ))
                throw new ArgumentNullException( nameof( friendlyName ) );
            services.AddSwaggerGen( options =>
            {
                options.SwaggerDoc( "v1", new Info { Title = friendlyName + " API", Version = "v1" } );
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
                if (xmlComments == default( IEnumerable<string> )) return;
                foreach (var path in xmlComments)
                {
                    if (File.Exists( path ))
                    {
                        options.IncludeXmlComments( path );
                    }
                }
            } );
            return services;
        }
    }
}
