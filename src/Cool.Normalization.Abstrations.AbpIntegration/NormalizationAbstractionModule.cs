using Abp.Dependency;
using Cool.Normalization;
using Cool.Normalization.Configuration;
using Cool.Normalization.Permissions;
using Cool.Normalization.Utilities;

namespace Abp.Modules
{
    /// <summary>
    /// 标准化定义的Abp模块。
    /// 通常不需要指定依赖此模块。
    /// </summary>
    public class NormalizationAbstractionModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<INormalizationConfiguration,
                NormalizationConfiguration>();

            IocManager.Register<IStdoutAuditStoreConfiguration,
               StdoutAuditStoreConfiguration>();

        }
        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IAssemblyNameResolver,
                AssemblyNameResolver>();

            IocManager.RegisterIfNot<IProxyPermissionChecker,
                NullProxyPermissionChecker>();

            IocManager.RegisterIfNot<IPermissionProvider,
                NullPermissionProvider>();

            IocManager.RegisterIfNot<IPermissionRegister,
                NullPermissionRegister>();

            IocManager.RegisterIfNot<IPermissionManager,
                NullPermissionManager>();

            
        }
    }
}
