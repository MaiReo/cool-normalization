using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace normalizationtests.Web.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging( l => l.SetMinimumLevel( LogLevel.Critical ) )
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
