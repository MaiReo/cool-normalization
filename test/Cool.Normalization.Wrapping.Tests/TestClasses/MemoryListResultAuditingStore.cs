using Cool.Normalization.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using Cool.Normalization.Models;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Abp.Dependency;

namespace Cool.Normalization.Tests
{
    public class MemoryListResultAuditingStore : IResultAuditingStore
    {
        public MemoryListResultAuditingStore()
        {
            _resultAuditingStore = new ConcurrentBag<NormalizationResponseBase>();
        }
        private ConcurrentBag<NormalizationResponseBase> _resultAuditingStore;
        public IEnumerable<NormalizationResponseBase> ResultAuditingStore => _resultAuditingStore;

        public void Save( NormalizationResponseBase normalizationResponse )
        {
            _resultAuditingStore.Add( normalizationResponse );
        }

        public Task SaveAsync( NormalizationResponseBase normalizationResponse )
        {
            this.Save( normalizationResponse );
            return Task.CompletedTask;
        }
    }
}
