using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.EntityFrameworkCore.Entities
{
    public class NormalizationResultAuditLog : Entity<long>
    {
        public string RequestId { get; set; }

        public string Result { get; set; }

    }
}
