namespace Cool.Normalization.Utilities
{
    public class Codes
    {

        public class Service
        {
            public const string Default = "00";
        }

        public class Api
        {
            public const string Default = "00";
        }

        public class Detail
        {
            public const string Default = "00";
        }
        /// <summary>
        /// 错误
        /// </summary>
        public class Level
        {

            /// <summary>
            /// 标识参数错误
            /// </summary>
            public const string ArgumentError = "01";
            /// <summary>
            /// 标识成功
            /// </summary>
            public const string Success = "00";
            /// <summary>
            /// 严重错误
            /// </summary>
            public const string Fatal = "99";
        }

    }
}
