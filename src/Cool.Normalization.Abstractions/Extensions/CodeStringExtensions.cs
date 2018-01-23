using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cool.Normalization
{
    public static class CodeStringExtensions
    {
        public static IReadOnlyDictionary<CodePart, string> AsCodes(this string code)
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
    }
}




