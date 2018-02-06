#region Version=1.0.4
/**
* 从权限模块中分离出来的单独通信模块。
* 此模块不在意是怎样集成到Cool.Normalization中并工作。
* 专注于实现与权限服务通信并传递权限注册和验证结果给权限模块
* 
* Initialize()
* 注册程序集到依赖注入容器
*/
#endregion Version

namespace Abp.Modules
{
    /// <summary>
    /// 权限的代理注册和权限的代理验证模块。
    /// 负责与权限微服务通信。
    /// </summary>
    [DependsOn(
        typeof( NormalizationPermissionModule ),
        typeof( NormalizationClientModule ) )]
    public class NormalizationPermissionRemoteProxyModule : AbpModule
    {

        public override void Initialize()
        {
            //检测配置中是否开启模块
            if (!Configuration.Modules.Normalization().IsPermissionEnabled)
            {
                return;
            }
            //将自己程序集内定义的类加入到依赖注入容器
            IocManager.RegisterAssemblyByConvention(
                typeof( NormalizationPermissionRemoteProxyModule ).Assembly );
            //将与权限模块通信的客户端内定义的类加入到依赖注入容器
            IocManager.RegisterAssemblyByConvention(
                typeof( cool.permission.client.Client.Configuration ).Assembly );
        }

    }
}
