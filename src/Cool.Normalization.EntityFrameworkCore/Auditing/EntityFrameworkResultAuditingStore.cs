using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cool.Normalization.Models;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Cool.Normalization.Utilities;
using Cool.Normalization.Auditing;
using Cool.Normalization.EntityFrameworkCore.Entities;

namespace Cool.Normalization.EntityFrameworkCore.Auditing
{
    public class EntityFrameworkResultAuditingStore : IResultAuditingStore, ITransientDependency
    {
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IRepository<NormalizationResultAuditLog, long> _normalizationResultAuditLogRepository;

        public EntityFrameworkResultAuditingStore( IRepository<NormalizationResultAuditLog, long> normalizationResultAuditLogRepository,
            IRequestIdAccessor requestIdAccessor )
        {
            this._normalizationResultAuditLogRepository = normalizationResultAuditLogRepository;
            this._requestIdAccessor = requestIdAccessor;
        }

        public void Save( NormalizationResponseBase normalizationResponse )
        {
            var requestId = _requestIdAccessor.RequestId;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject( normalizationResponse );
            _normalizationResultAuditLogRepository.Insert( new NormalizationResultAuditLog
            {
                RequestId = requestId,
                Result = json,
            } );
        }

        public async Task SaveAsync( NormalizationResponseBase normalizationResponse )
        {
            var requestId = _requestIdAccessor.RequestId;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject( normalizationResponse );
            await _normalizationResultAuditLogRepository.InsertAsync( new NormalizationResultAuditLog
            {
                RequestId = requestId,
                Result = json,
            } );
        }
    }
}
