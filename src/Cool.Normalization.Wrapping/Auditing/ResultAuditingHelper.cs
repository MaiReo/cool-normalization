#region 程序集 Version=1.0.6
/*
 * ResultAuditingHelper为Action的输出审计提供记录行为
 * 
 * 配置：
 *    INormalizationConfiguration
 *    RequestIdHeaderName ： 用于指定RequestId的key值，默认为X-Cool-RequestId。不建议修改此配置。
 *    
 * 成员：
 *  IResultAuditingStore，实际的存储实现类
 * 
 * 方法：
 *  Save
 *  SaveAsync  ： 存储日志方法，内部调用IResultAuditingStore 实现存储
 *  ShouldSaveAudit ： 通过配置判断是否应该记录日志
 *  
 * 使用场景
 *  该类负责判断是否需要记录日志，并且将记录日志的行为委托给IResultAuditingStore实现类。
 */
#endregion

#region Version=1.1.0
/**
 * ShouldSaveAudit(MethodInfo, bool)
 * 增加了对INormalizationConfiguration.IsStandardOutputAuditLogEnabled的判断
*/
#endregion Version

using Abp.Dependency;
using Abp.Domain.Uow;
using Cool.Normalization.Configuration;
using Cool.Normalization.Models;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

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
            if (!_normalizationConfiguration.IsStandardOutputAuditLogEnabled)
                return false;
            if (methodInfo == null)
                return false;

            if (methodInfo.IsPublic == false)
                return false;

            if (methodInfo.IsDefined( typeof( ResultAuditedAttribute ), true ))
                return true;

            if (methodInfo.IsDefined( typeof( DisableResultAuditingAttribute ), true ))
                return false;

            var classType = methodInfo.DeclaringType;

            if (classType != null)
            {
                if (classType.GetTypeInfo().IsDefined( typeof( ResultAuditedAttribute ), true ))
                    return true;

                if (classType.GetTypeInfo().IsDefined( typeof( DisableResultAuditingAttribute ), true ))
                    return false;
            }
            return defaultValue;
        }
    }
}
