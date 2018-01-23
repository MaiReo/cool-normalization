using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace normalizationtests.EntityFrameworkCore
{
    public class normalizationtestsDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public normalizationtestsDbContext(DbContextOptions<normalizationtestsDbContext> options) 
            : base(options)
        {

        }
    }
}
