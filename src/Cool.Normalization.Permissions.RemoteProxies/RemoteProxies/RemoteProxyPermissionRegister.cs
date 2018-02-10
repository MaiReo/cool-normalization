#region Version=1.0.4
/**
* 与权限服务通信的注册权限实现类。
* 
* _permissionApi
* 只读字段。在构造函数中被赋值。一个指向与权限服务通信的实例对象的引用。
* 
* RemoteProxyPermissionRegister(IPermissionApi)
* 构造函数。依赖一个与权限服务通信的通信的对象并保存到成员_permissionApi。
*
* Register(string, string, IEnumerable{CoolPermission})
* 注册权限定义。详见IPermissionRegister接口
* 将参数传递给权限服务等待处理结果。
* 权限注册成功时不引发异常。
* 其他情况则引发类型为NormalizationException的异常。
*
* 可能由此方法引发的Code值
*   权限服务返回非成功：“03000002”
*   程序异常：“99000001”
*
* 可能引发的异常：
*   System.NullReferenceException
* 
* List{PermissionDto} MapToDto(IEnumerable{CoolPermission})
* 将注册权限定义的参数转换为权限服务所需参数的方法。
* 将每一个权限按照名字中包含的“.”的数量作为参数“Level”的值。
* 可能引发的异常：
*   System.NullReferenceException
*
*/
#endregion Version
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
        ///<summary>
        ///见<see cref="IPermissionRegister.Register(string, string, IEnumerable{CoolPermission})"/>
        ///</summary>
        ///<exception cref="Cool.Normalization.NormalizationException" />
        ///<exception cref="System.NullReferenceException" />
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
                    //message: ex.Message.Replace( "\r", string.Empty ).Replace( "\n", string.Empty ) );
                    message : ex.StackTrace);
            }
        }

        ///<summary>
        ///
        ///</summary>
        ///<exception cref="System.NullReferenceException" />
        public List<PermissionDto> MapToDto(
            IEnumerable<CoolPermission> permissions)
        {
            var list = new List<PermissionDto>(permissions.Count());

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
