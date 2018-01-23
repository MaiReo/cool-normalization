using Abp.Dependency;
using Cool.Normalization.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace normalizationtests.Web.Configuration
{
    public class DebugPermissionProvider : IPermissionProvider, ISingletonDependency
    {

        public const string 全部 = "All";

        public const string 调试权限 = "All.Debug";

        public const string 添加调试权限 = "All.Debug.Add";

        public const string 修改调试权限 = "All.Debug.Update";

        public const string 删除调试权限 = "All.Debug.Delete";

        public const string 发布权限 = "All.Release";

        public const string 添加发布权限 = "All.Release.Add";

        public const string 修改发布权限 = "All.Release.Update";

        public const string 删除发布权限 = "All.Release.Delete";
    }
}
