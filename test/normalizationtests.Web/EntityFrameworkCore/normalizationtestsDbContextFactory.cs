using normalizationtests.Configuration;
using normalizationtests.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace normalizationtests.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class normalizationtestsDbContextFactory : IDesignTimeDbContextFactory<normalizationtestsDbContext>
    {
        public normalizationtestsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<normalizationtestsDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(normalizationtestsConsts.ConnectionStringName)
            );

            return new normalizationtestsDbContext(builder.Options);
        }
    }
}