#region Version=1.0.9
/**
* 输出审计的定义
*/
#endregion Version
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.Auditing
{
    ///<summary>
    /// 输出审计存储
    /// </summary>
    public interface IResultAuditingStore
    {
        ///<summary>
        /// 保存输出审计
        /// </summary>
        /// <param name="normalizationResponse">输出封装的模型类</param>
        void Save( Models.NormalizationResponseBase normalizationResponse );
        
        ///<summary>
        /// 异步保存输出审计
        /// </summary>
        /// <param name="normalizationResponse">输出封装的模型类</param>
        Task SaveAsync( Models.NormalizationResponseBase normalizationResponse );
    }
}
