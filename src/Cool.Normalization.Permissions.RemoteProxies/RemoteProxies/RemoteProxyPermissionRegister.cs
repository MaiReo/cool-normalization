using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Localization;
using cool.permission.client.Api;
using cool.permission.client.Model;

namespace Cool.Normalization.Permissions
{
    public class RemoteProxyPermissionRegister : IPermissionRegister, ISingletonDependency
    {
        private readonly IPermissionApi _permissionApi;

        public RemoteProxyPermissionRegister(IPermissionApi permissionApi)
        {
            this._permissionApi = permissionApi;
        }
        public void Register(string name, string displayName, IEnumerable<CoolPermission> permissions)
        {
            var input = new RegisterInput( name, displayName, MapToDto( permissions ) );
            try
            {
                var response = _permissionApi.Register( input );
                if (!response.IsSuccess())
                {
                    throw new NormalizationException( "02", "03", "无法访问权限服务" );
                }
            }
            catch (Exception ex) when (!(ex is NormalizationException))
            {
                throw new NormalizationException( "01", 
                    message: ex.Message.Replace( "\r", string.Empty ).Replace( "\n", string.Empty ) );
            }
        }


        public List<PermissionDto> MapToDto(
            IEnumerable<CoolPermission> permissions)
        {
            var list = new List<PermissionDto>();

            foreach (var per in permissions)
            {
                var level = per.Name.Count(c => c == '.');
                var dto = new PermissionDto(per.Name,
                    per.DisplayName, level);
                list.Add(dto);
            }
            return list;
        }
    }
}
