using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Client
{
    /// <summary>
    /// 微服务客户端在发出调用请求前设置请求唯一识别符
    /// </summary>
    public interface IRequestIdSetter
    {
        /// <summary>
        /// 设置请求唯一识别符
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        bool SetRequestId( Dictionary<String, String> headers );
    }
}
