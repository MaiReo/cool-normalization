#region Version=1.0.4
/*
* 与权限服务通信的鉴权实现类。
* 
* _permissionApi
* 只读字段。在构造函数中被赋值。一个指向与权限服务通信的实例对象的引用。
* 
* RemoteProxyPermissionChecker(IPermissionApi)
* 构造函数。依赖一个与权限服务通信的通信的对象并保存到成员_permissionApi。
*
* Task<bool> IsGrantedAsync(long, string, string)
* 鉴权定义。详见IProxyPermissionChecker接口
* 将参数传递给权限服务等待处理结果。
* 权限鉴定成功时返回true。
* 其他情况则引发类型为NormalizationException的异常。
* 可能由此方法引发的Code值
*   未授权：“03{SERVICE_CODE}{API_CODE}01”
*   权限服务返回非成功：“03{SERVICE_CODE}{API_CODE}02”
*   通信异常：“03{SERVICE_CODE}{API_CODE}03”
*   程序异常：“99{SERVICE_CODE}{API_CODE}01”
* 
*/
#endregion Version
using cool.permission.client.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using cool.permission.client.Model;
using Cool.Normalization.Utilities;
using Abp.Dependency;

namespace Cool.Normalization.Permissions
{
    public class RemoteProxyPermissionChecker : IProxyPermissionChecker, ISingletonDependency
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
