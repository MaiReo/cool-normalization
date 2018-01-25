using Abp.Application.Services;
using Abp.Authorization;
using Abp.Runtime.Session;
using cool.permission.client.Api;
using Cool.Normalization;
using RequiredAttribute = Cool.Validations.RequiredAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [AbpAuthorize( "All" )]
        public dynamic IdentityWithPermission()
        {
            return new { AccountId = AbpSession.GetAccountId() };
        }

    }

    [Code( "03" )]
    public class AccountAppService : normalizationtestsAppServiceBase, IApplicationService
    {

        [Code( "03" )]
        public Task ChangePassword(ChangePasswordInput input)
        {
            try
            {
                
            }
            catch (Exception ex)
            {

                throw ex.WithCode("99");
            }
            return Task.CompletedTask;
        }

        public async  Task<bool> TestAbpSession()
        {
            return AbpSession.GetType().Name == "AccessTokenAbpSession";
        }
    }


    public class ChangePasswordInput
    {
        [Code( "01" )]
        [Display( Name = "艾迪" )]
        [Required( ErrorMessage = "请填写此字段:{0}" )]
        public long Id { get; set; }

        [Code( "02" )]
        [Display( Name = "帕斯伍德" )]
        [Required( ErrorMessage = "请填写此字段:{0}" )]
        public string Password { get; set; }
    }
}
