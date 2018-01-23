using Abp;
using Abp.Authorization;
using Abp.Runtime.Session;
using System;
using System.Reflection;
using System.Threading.Tasks;


namespace Cool.Normalization.Permissions
{
    public class ProxyPermissionChecker : IPermissionChecker
    {
        private readonly IAssemblyName _assemblyName;
        public ProxyPermissionChecker(IAssemblyNameResolver assemblyNameResolver)
        {
            _assemblyName = assemblyNameResolver.ResolveEntryName( Assembly.GetEntryAssembly() );
        }

        public IAbpSession AbpSession { get; set; }

        public IRemoteProxyPermissionChecker Proxy { get; set; }


        Task<bool> IPermissionChecker.IsGrantedAsync(string permissionName)
            => this.IsGrantedAsync( AbpSession.GetAccountId(), permissionName );

        Task<bool> IPermissionChecker.IsGrantedAsync(UserIdentifier user, string permissionName)
            => this.IsGrantedAsync( user.UserId, permissionName );

        private Task<bool> IsGrantedAsync(long accountId, string permissionName)
        {
            return Proxy?.IsGrantedAsync( accountId, permissionName, _assemblyName?.UniqueName )
                ?? Task.FromResult( true );

        }

    }
}
