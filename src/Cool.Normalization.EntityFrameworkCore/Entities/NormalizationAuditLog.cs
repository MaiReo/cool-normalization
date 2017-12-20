using Abp.Auditing;
using Abp.Domain.Entities;
using System;

namespace Cool.Normalization.EntityFrameworkCore.Entities
{
    public class NormalizationAuditLog : Entity<long>
    {
        public string RequestId { get; set; }

        public int? TenantId { get; set; }

        public long? UserId { get; set; }


        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public string ClientIpAddress { get; set; }

        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }

        public string CustomData { get; set; }

        public string Exception { get; set; }

        public static NormalizationAuditLog MapFromAuditInfo( AuditInfo auditInfo )
        {
            if (auditInfo == null)
            {
                return NormalizationAuditLog.New();
            }
            return new NormalizationAuditLog
            {
                BrowserInfo = auditInfo.BrowserInfo,
                ClientIpAddress = auditInfo.ClientIpAddress,
                ClientName = auditInfo.ClientName,
                CustomData = auditInfo.CustomData,
                Exception = auditInfo.Exception?.ToString(),
                ExecutionDuration = auditInfo.ExecutionDuration,
                ExecutionTime = auditInfo.ExecutionTime,
                MethodName = auditInfo.MethodName,
                Parameters = auditInfo.Parameters,
                ServiceName = auditInfo.ServiceName,
                TenantId = auditInfo.TenantId,
                UserId = auditInfo.UserId
            };
        }

        public static NormalizationAuditLog New() => new NormalizationAuditLog();

    }
}