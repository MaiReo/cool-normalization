using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NormalizationServiceCollectionExtensions
    {
        public static string DisplayName { get; private set; }
        static NormalizationServiceCollectionExtensions()
        {
            DisplayName = Assembly.GetEntryAssembly().GetName().Name;
        }
        public static IServiceCollection AddNormalization(this IServiceCollection services,
            string displayName = default,
            bool? addWrapping = default,
            bool? addSwagger = default,
            bool? addAuth = default)
        {
            if (string.IsNullOrWhiteSpace( displayName ))
            {
                displayName = DisplayName;
            }
            else
            {
                DisplayName = displayName;
            }
            if (addWrapping != false)
            {
                services.AddNormalizationWrapping();
            }
            if (addAuth == true)
            {
                services.AddAuth();
            }
            services.AddSwaggerEtc( displayName, addAuth == true );
            return services;
        }
    }
}
