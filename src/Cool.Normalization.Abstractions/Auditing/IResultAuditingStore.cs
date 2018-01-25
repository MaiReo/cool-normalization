using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.Auditing
{
    public interface IResultAuditingStore
    {
        void Save( Models.NormalizationResponseBase normalizationResponse );
        Task SaveAsync( Models.NormalizationResponseBase normalizationResponse );
    }
}
