using Cool.Normalization.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cool.Normalization
{
    public static class NormalizationResponseExtensions
    {

        [Flags]
        private enum CodePart
        {
            /// <summary>
            /// Level part.This is the 1st part of a full code.
            /// </summary>
            Level = 1 << 1,
            /// <summary>
            /// Service part.This is the 2st part of a full code.
            /// </summary>
            Service = 1 << 2,
            /// <summary>
            /// Api part. This is the 3rd part of a full code.
            /// </summary>
            Api = 1 << 3,
            /// <summary>
            /// Detail part. This is the 4th part, the last part of a full code.
            /// </summary>
            Detail = 1 << 4,
        }
        private static IReadOnlyDictionary<CodePart, string> AsCodes(this string code)
        {
            if (string.IsNullOrWhiteSpace( code ))
            {
                return null;
            }

            if (code.Length != 8)
            {
                return null;
            }
            if (code.Any( c => !char.IsDigit( c ) ))
            {
                return null;
            }
            return new Dictionary<CodePart, string>
            {
                { CodePart.Level ,code.Substring(0,2) },
                { CodePart.Service ,code.Substring(2,2) },
                { CodePart.Api ,code.Substring(4,2) },
                { CodePart.Detail ,code.Substring(6,2) },
            };
        }


        public static bool IsSuccess(this NormalizationResponse response)
        {
            return response.ErrorLevel() == "00";
        }

        public static string ErrorLevel(this NormalizationResponse response)
        {
            var code = default( string );
            response.Code.AsCodes()?.TryGetValue( CodePart.Level, out code );
            return code;
        }

        public static string ServiceCode(this NormalizationResponse response)
        {
            var code = default( string );
            response.Code.AsCodes()?.TryGetValue( CodePart.Service, out code );
            return code;
        }

        public static string ApiCode(this NormalizationResponse response)
        {
            var code = default( string );
            response.Code.AsCodes()?.TryGetValue( CodePart.Api, out code );
            return code;
        }

        public static string DetailCode(this NormalizationResponse response)
        {
            var code = default( string );
            response.Code.AsCodes()?.TryGetValue( CodePart.Detail, out code );
            return code;
        }
    }
}
