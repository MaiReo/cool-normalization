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
    }
}