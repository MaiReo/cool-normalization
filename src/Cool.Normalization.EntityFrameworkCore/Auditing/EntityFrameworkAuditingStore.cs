using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Castle.Core.Logging;
using Cool.Normalization.EntityFrameworkCore.Entities;
using Cool.Normalization.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.EntityFrameworkCore.Auditing
{
    public class EntityFrameworkAuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IRepository<NormalizationAuditLog, long> _auditLogRepository;

        public IRequestIdAccessor RequestIdAccessor { get; set; }

        public ILogger Logger { get; set; }

        public EntityFrameworkAuditingStore( IRepository<NormalizationAuditLog, long> auditLogRepository )
        {
            this._auditLogRepository = auditLogRepository;

            Logger = NullLogger.Instance;
        }
        public async Task SaveAsync( AuditInfo auditInfo )
        {
            Logger.Info( auditInfo.ToString() );
            var log = NormalizationAuditLog.MapFromAuditInfo( auditInfo );
            log.RequestId = RequestIdAccessor?.RequestId;
            await _auditLogRepository.InsertAsync( log );
        }
    }
}
