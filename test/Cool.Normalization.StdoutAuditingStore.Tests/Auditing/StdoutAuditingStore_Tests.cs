using Abp.Auditing;
using Cool.Normalization.Auditing;
using Cool.Normalization.Configuration;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class StdoutAuditingStore_Tests : StdoutAuditingStoreTestBase
    {
        private readonly TestConsoleLogger _logger;
        private readonly INormalizationConfiguration _normalizationConfiguration;
        public const string const_request_id = "const_request_id";
        public StdoutAuditingStore_Tests()
        {
            _logger = Resolve<TestConsoleLogger>();
            _normalizationConfiguration
                = Resolve<INormalizationConfiguration>();
            TestHttpContext.Current.Request.Headers
                [_normalizationConfiguration.RequestIdHeaderName]
                = const_request_id;
        }

        [Fact]
        public void DI_Test()
        {
            var auditingStore = Resolve<IAuditingStore>();
            auditingStore.ShouldBeOfType<StdoutAuditingStore>();

            var resultAuditingStore = Resolve<IResultAuditingStore>();
            resultAuditingStore.ShouldBeOfType<StdoutResultAuditingStore>();
        }

        [Fact]
        public async Task SaveAsync_Test()
        {
            var auditingStore = Resolve<IAuditingStore>();
            var auditInfo = new AuditInfo
            {
                ExecutionTime = DateTime.UtcNow,
                ExecutionDuration = 300,
                ClientIpAddress = "[::1]",
                MethodName = "Me",
                ServiceName = "Svc",
            };
            await auditingStore.SaveAsync(auditInfo);
            var message = _logger.GetLastMessage();
            var correctMessage = $"AUDIT-IN^{const_request_id}^" +
                $"{auditInfo.ExecutionTime.ToString("yyyy-MM-dd-HH:mm:ssz")}^" +
                $"Svc^Me^300^NULL^NULL^[::1]^NULL^NULL^NULL^NULL^NULL^NULL^NULL";
            message.ShouldBe(correctMessage);
            message.Split("^").Length.ShouldBe(16);
        }

        [Fact]
        public async Task SaveAsync_HasException_Test()
        {
            Exception exception = null;
            try
            {
                throw new Exception("EEEERROR");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var auditingStore = Resolve<IAuditingStore>();
            var auditInfo = new AuditInfo
            {
                ExecutionTime = DateTime.UtcNow,
                ExecutionDuration = 300,
                ClientIpAddress = "[::1]",
                MethodName = "Me",
                ServiceName = "Svc",
                Exception = exception,
            };
            await auditingStore.SaveAsync(auditInfo);
            var message = _logger.GetLastMessage();
            var correctMessageStart = $"AUDIT-IN^{const_request_id}^" +
                $"{auditInfo.ExecutionTime.ToString("yyyy-MM-dd-HH:mm:ssz")}^" +
                $"Svc^Me^300^NULL^NULL^[::1]^Exception^EEEERROR^{typeof(StdoutAuditingStore_Tests).FullName}^{nameof(SaveAsync_HasException_Test)}^NULL^";
            message.Split("^").Length.ShouldBe(16);
            message.ShouldStartWith(correctMessageStart);
            message.Length.ShouldBeGreaterThan(correctMessageStart.Length);

        }

        [Fact]
        public void Save_HasException_Test()
        {
            Exception exception = null;
            try
            {
                throw new Exception("EEEERROR");
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            var auditingStore = Resolve<IAuditingStore>();
            var auditInfo = new AuditInfo
            {
                ExecutionTime = DateTime.UtcNow,
                ExecutionDuration = 300,
                ClientIpAddress = "[::1]",
                MethodName = "Me",
                ServiceName = "Svc",
                Exception = exception,
            };
            auditingStore.Save(auditInfo);
            var message = _logger.GetLastMessage();
            var correctMessageStart = $"AUDIT-IN^{const_request_id}^" +
                $"{auditInfo.ExecutionTime.ToString("yyyy-MM-dd-HH:mm:ssz")}^" +
                $"Svc^Me^300^NULL^NULL^[::1]^Exception^EEEERROR^{typeof(StdoutAuditingStore_Tests).FullName}^{nameof(Save_HasException_Test)}^NULL^";
            message.Split("^").Length.ShouldBe(16);
            message.ShouldStartWith(correctMessageStart);
            message.Length.ShouldBeGreaterThan(correctMessageStart.Length);

        }

    }
}
