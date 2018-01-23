using Abp.Application.Services;
using Abp.Authorization;
using Abp.Runtime.Session;
using cool.permission.client.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace normalizationtests.Application
{
    
    public class DebugAppService : normalizationtestsAppServiceBase, IApplicationService
    {
        [AbpAuthorize]
        public dynamic Identity()
        {
            return new { AccountId = AbpSession.GetAccountId() };
        }

        [AbpAuthorize("SomePerm")]
        public dynamic IdentityWithPermission()
        {
            return new { AccountId = AbpSession.GetAccountId() };
        }
    }
}
