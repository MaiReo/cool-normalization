using Abp.Modules;
using Abp.Runtime.Session;
using Cool.Normalization.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Configuration.Startup;
using System.IdentityModel.Tokens.Jwt;
using Abp.Authorization;
using Abp.Dependency;
using Abp.MultiTenancy;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Client;
using cool.permission.client.Api;

namespace Cool.Normalization
{
    [DependsOn(
        typeof( NormalizationPermissionModule ),
        typeof( NormalizationClientModule ) )]
    public class NormalizationPermissionRemoteProxyModule : AbpModule
    {

        public override void PreInitialize()
        {
            Configuration.ReplaceService<IPermissionRegister, RemoteProxyPermissionRegister>();
            IocManager.Register<IRemoteProxyPermissionChecker, RemoteProxyPermissionChecker>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationPermissionRemoteProxyModule ).Assembly );
            IocManager.RegisterAssemblyByConvention( typeof( cool.permission.client.Client.Configuration ).Assembly );
        }
    }
}
