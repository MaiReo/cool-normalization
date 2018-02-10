using Abp;
using Abp.Modules;
using Cool.Normalization.Permissions;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Cool.Normalization.Permissions.Tests
{
    public class PermissionManager_Tests : NormalizationPermissionTestBase
    {
        private readonly FakePermissionRegister _fakePermissionRegister;

        private readonly PermissionManager _permissionManager;
        public PermissionManager_Tests()
        {
            ResolveSelf(ref _fakePermissionRegister);
            ResolveSelf(ref _permissionManager);
        }

        [Fact( DisplayName = "×¢²áÈ¨ÏÞ" )]
        public void Register_Test()
        {
            _permissionManager.Register();
            _fakePermissionRegister.RegisterInputs.ShouldNotBeEmpty();
            var input = _fakePermissionRegister.RegisterInputs.First();
            //testhost : when running under cli "dotnet test"
            new [] { "testhost",typeof(PermissionManager_Tests).Assembly.GetName().Name}.ShouldContain(input.Name);
            input.Permissions.ShouldNotBeEmpty();
            input.Permissions.Count().ShouldBe(1);
            input.Permissions.First().Name.ShouldBe("Fake");
            input.Permissions.First().DisplayName.ShouldBe("Fake");
        }
    }
}