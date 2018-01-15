using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages.Tests
{
    public class TestDbContext : AbpDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base( options )
        {
        }

        public DbSet<TestMessageEntity> TestMessageEntities { get; set; }
    }
}
