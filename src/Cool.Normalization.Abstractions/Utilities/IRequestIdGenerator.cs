using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization
{
    /// <summary>
    /// 请求唯一识别符生成器
    /// </summary>
    public interface IRequestIdGenerator
    {
        /// <summary>
        /// 获取当前请求唯一识别符
        /// </summary>
        string RequestId { get; }
    }
}
