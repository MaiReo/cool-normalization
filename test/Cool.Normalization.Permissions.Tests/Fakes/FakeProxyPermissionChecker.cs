using System.Threading.Tasks;
using Abp.Dependency;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System;
using System.Reflection;

namespace Cool.Normalization.Permissions
{
    public class FakeProxyPermissionChecker : IProxyPermissionChecker, ISingletonDependency
    {
        public FakeProxyPermissionChecker()
        {
            _permissions = new ConcurrentDictionary<string, ConcurrentDictionary<long, ConcurrentBag<string>>>();
        }
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<long, ConcurrentBag<string>>> _permissions;
        public Task<bool> IsGrantedAsync(long accountId, string permissionName, string uniqueName = null)
        {
            var permissions = _permissions.Values.SelectMany(p => p);

            if (!string.IsNullOrWhiteSpace(uniqueName))
                permissions = _permissions.GetValueOrDefault(uniqueName);

            var isGranted = permissions
                ?.Where(p => p.Key == accountId)
                ?.Any(p => p.Value.Any(
                    name => name == permissionName)) == true;

            return Task.FromResult(isGranted);
        }

        public void GrantPermission(long accountId, string permissionName, string uniqueName = null)
        {
            if (string.IsNullOrWhiteSpace(uniqueName))
                uniqueName = Assembly.GetEntryAssembly().GetName().Name;
            var subPermission = _permissions.GetOrAdd(uniqueName, new ConcurrentDictionary<long, ConcurrentBag<string>>());
            var names = subPermission.GetOrAdd(accountId, new ConcurrentBag<string>());
            if (names.Contains(permissionName))
                throw new InvalidOperationException($"Permission \"{permissionName}\" already granted for {accountId} ");
            names.Add(permissionName);
        }
    }
}