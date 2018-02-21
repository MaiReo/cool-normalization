using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using normalizationtests.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace normalizationtests.Web.Startup
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Configure DbContext
            services.AddAbpDbContext<normalizationtestsDbContext>( options =>
             {
                 DbContextOptionsConfigurer.Configure( options.DbContextOptions, options.ConnectionString );
             } );

            services.AddMvc();

            services.AddNormalization( "Cool Normalization Tests", 
                addAuth: true,swaggerDocs: new[] { "normalizationtests.Web.xml" } );

            //Configure Abp and Dependency Injection
            return services.AddAbp<normalizationtestsWebModule>( options =>
             {
                 //Configure Log4Net logging
                 options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                      f => f.UseAbpLog4Net().WithConfig( "log4net.config" )
                  );
             } );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework

            app.UseNormalization( useAuth: true );

            app.UseMvcWithDefaultRoute();
        }
    }
}
