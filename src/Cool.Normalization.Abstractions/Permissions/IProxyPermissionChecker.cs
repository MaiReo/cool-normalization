using System.Threading.Tasks;

namespace Cool.Normalization.Permissions
{
    public interface IProxyPermissionChecker
    {
        Task<bool> IsGrantedAsync(long accountId, string permissionName, string uniqueName = default( string ));
    }
}
