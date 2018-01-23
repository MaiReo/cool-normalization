using Microsoft.EntityFrameworkCore;

namespace normalizationtests.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<normalizationtestsDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for normalizationtestsDbContext */
            dbContextOptions.UseMySql(connectionString);
        }
    }
}
