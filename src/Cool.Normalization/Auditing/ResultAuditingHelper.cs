using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using Cool.Normalization.Models;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Domain.Uow;
using Cool.Normalization.Configuration;

namespace Cool.Normalization.Auditing
{
    public class ResultAuditingHelper : IResultAuditingHelper, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly INormalizationConfiguration _normalizationConfiguration;

        public IResultAuditingStore AuditingResultStore { get; set; }


        public ResultAuditingHelper( IUnitOfWorkManager unitOfWorkManager,
            INormalizationConfiguration normalizationConfiguration )
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this._normalizationConfiguration = normalizationConfiguration;

            AuditingResultStore = SimpleLogResultAuditingStore.Instance;
        }

        public void Save( NormalizationResponseBase normalizationResponse )
        {
            using (var uow = _unitOfWorkManager.Begin( TransactionScopeOption.Suppress ))
            {
                AuditingResultStore.Save( normalizationResponse );
                uow.Complete();
            }
        }

        public async Task SaveAsync( NormalizationResponseBase normalizationResponse )
        {
            using (var uow = _unitOfWorkManager.Begin( TransactionScopeOption.Suppress ))
            {
                await AuditingResultStore.SaveAsync( normalizationResponse );
                await uow.CompleteAsync();
            }
        }

        public bool ShouldSaveAudit( MethodInfo methodInfo, bool defaultValue = true )
        {
            if (!_normalizationConfiguration.ResultAuditing)
            {
                return false;
            }
            if (methodInfo == null)
            {
                return false;
            }

            if (!methodInfo.IsPublic)
            {
                return false;
            }

            if (methodInfo.IsDefined( typeof( ResultAuditedAttribute ), true ))
            {
                return true;
            }

            if (methodInfo.IsDefined( typeof( DisableResultAuditingAttribute ), true ))
            {
                return false;
            }

            var classType = methodInfo.DeclaringType;

            if (classType != null)
            {
                if (classType.GetTypeInfo().IsDefined( typeof( ResultAuditedAttribute ), true ))
                {
                    return true;
                }

                if (classType.GetTypeInfo().IsDefined( typeof( DisableResultAuditingAttribute ), true ))
                {
                    return false;
                }
            }
            return defaultValue;
        }
    }
}
