using cool.permission.client.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using cool.permission.client.Model;
using Cool.Normalization.Utilities;

namespace Cool.Normalization.Permissions
{
    public class RemoteProxyPermissionChecker : IRemoteProxyPermissionChecker
    {
        private readonly IPermissionApi _permissionApi;

        public RemoteProxyPermissionChecker(IPermissionApi permissionApi)
        {
            _permissionApi = permissionApi;
        }

        public async Task<bool> IsGrantedAsync(long accountId, string permissionName, string uniqueName = null)
        {
            var grantOutput = default( IsGrantOutput );
            try
            {
                var response = await _permissionApi.IsGrantAsync( new IsGrantInput( accountId, permissionName, uniqueName ) );
                //if (!response.Code.StartsWith( Codes.Level.Success ))
                if (!response.IsSuccess())
                {
                    throw new NormalizationException( "02", "03", "无法访问权限服务" );
                }
                grantOutput = response.Data;
            }
            catch (Exception ex) when (!(ex is NormalizationException))
            {
                throw new NormalizationException( "01", message: ex.Message.Replace( "\r", string.Empty ).Replace( "\n", string.Empty ) );
            }

            if (grantOutput == default( IsGrantOutput ))
            {
                throw new NormalizationException( "03", "03", "权限服务返回值不是期望的" );
            }
            if (grantOutput.IsGranted == false)
            {
                throw new NormalizationException( "01", "03", "权限服务返回未授权" );
            }
            return true;
        }
    }
}
