using Cool.Normalization.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Auditing
{
    public interface IAuditingDbContext<in TDbContext> where TDbContext : DbContext
    {
        DbSet<NormalizationAuditLog> NormalizationAuditLogs { get; set; }
    }
}
