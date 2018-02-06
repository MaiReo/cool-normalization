using Abp.Runtime.Session;
using System;
using System.Security.Claims;
using Abp.Dependency;

namespace Cool.Normalization.Permissions
{
    public class FakePrincipalAccessor : IPrincipalAccessor, ISingletonDependency
    {
        public static FakePrincipalAccessor Instance { get; private set; }
        public FakePrincipalAccessor() => Instance = this;
        static FakePrincipalAccessor() => Instance = new FakePrincipalAccessor();
        public ClaimsPrincipal Principal { get; set; }
    }
}