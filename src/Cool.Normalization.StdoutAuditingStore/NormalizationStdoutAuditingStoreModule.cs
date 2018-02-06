#region Version=1.1.4
/**
* 名字虽然写着什么标准输出，但是只是个格式化字符串的类
* 实际输出依赖的是运行时的实现ILogger接口的类的实例的实际行为。
* 需要配置log4net.config以支持输出审计日志到标准输出流。
* 
* 输入审计：也称为入参审计，输入参数审计
* 输出审计：也称为出参审计，输出参数审计
*
* NormalizationStdoutAuditingStoreModule
* 格式化输入审计日志交给ILogger以Info级别输出。
* 格式化输出审计日志交给ILogger以Info级别输出。
* 
* Initialize()
* 注册程序集到依赖注入容器
*
* 可能引发的Code值
*
* 可能引发的异常
* 
*/
#endregion Version
using Abp.Auditing;
using Cool.Normalization;
using Cool.Normalization.Auditing;

namespace Abp.Modules
{
    /// <summary>
    /// 标准输出审计日志Abp模块
    /// </summary>
    [DependsOn(typeof(NormalizationAbstractionModule))]
    public class NormalizationStdoutAuditingStoreModule : AbpModule
    {
        public override void Initialize()
        {
            //检测配置中是否开启模块
            if (!Configuration.Modules.Normalization()
                    .IsStandardOutputAuditLogEnabled)
            {
                return;
            }
            
            //替换输入审计默认实现
            IocManager.ReplaceService<IAuditingStore,
                StdoutAuditingStore>();
            //替换输出审计默认实现
            IocManager.ReplaceService<IResultAuditingStore,
                StdoutResultAuditingStore>();
            
            IocManager.RegisterAssemblyByConvention(
                typeof( NormalizationStdoutAuditingStoreModule ).Assembly );
        }

    }
}
