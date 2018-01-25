#region 程序集 Version=1.0.6
/*
 * 此程序集将所有模块依赖的抽象定义单独提取，几乎没有任何实际功能
 */
#endregion

namespace Cool.Normalization.Configuration
{
    public interface INormalizationConfiguration
    {
        /// <summary>
        /// RequestId在Http头的名字
        /// 默认值:X-Cool-RequestId
        /// </summary>
        string RequestIdHeaderName { get; set; }

        /// <summary>
        /// 是否开启包装功能。
        /// 默认值:true
        /// </summary>
        bool IsWrappingEnabled { get; set; }

        /// <summary>
        /// 是否开启身份和权限功能。
        /// 默认值:true
        /// </summary>
        bool IsPermissionEnabled { get; set; }

        /// <summary>
        /// 是否开启标准输出审计日志功能。
        /// 默认值:true
        /// </summary>
        bool IsStandardOutputAuditLogEnabled { get; set; }

        /// <summary>
        /// 是否开启消息功能。
        /// 默认值: false
        /// </summary>
        bool IsMessageEnabled { get; set; }
    }
}