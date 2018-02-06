using System.Threading.Tasks;

namespace Cool.Normalization.Permissions
{
    ///<summary>
    /// 实现<see cref="IsGrantedAsync" />以提供鉴权
    ///</summary>
    public interface IProxyPermissionChecker
    {
        ///<summary>
        /// 鉴权定义。
        ///<param name="accountId">帐号id</param>
        ///<param name="permissionName">权限识别名</param>
        ///<param name="uniqueName">唯一识别名</param>
        ///<return>鉴权通过应该返回true</return>
        ///<exception cref="NormalizationException">如果需要必须引发此类型异常</exception>
        ///</summary>
        Task<bool> IsGrantedAsync(long accountId, string permissionName, string uniqueName = default(string));
    }
}
